using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Modal;
using ImageService.Server;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ImageService.Controller.Handlers
{
    public class DirectoyHandler : IDirectoryHandler
    {
        #region Members
        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;
        private string path;
        private IImageController controller;
        private FileSystemWatcher watcher;
        private ILoggingService logging;
        private DateTime lastRead;
        #endregion

        /**
         * constructor for DirectoyHandler
         */
        public DirectoyHandler(string path, IImageController controller1, ILoggingService logging1)
        {
            this.path = path;
            this.logging = logging1;
            this.controller = controller1;
            this.watcher = new FileSystemWatcher(path);
        }

        /**
         * when the directory is changing, send the pictures to the correct path.
         */
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            string[] args = new string[1];
            args[0] = e.FullPath;
            string path = args[0];
            bool result;
            DateTime lastWriteTime = File.GetLastWriteTime(e.FullPath);
            MessageTypeEnum typeMsg = new MessageTypeEnum();
            if (lastWriteTime != lastRead)
            {
                string resultMsg = "could'nt find picture...";
                //string fileEnding = Path.GetExtension(path);
                if (isCorrectEnding(path))
                {
                    resultMsg = controller.ExecuteCommand((int)CommandEnum.NewFileCommand, args, out result);
                    if (result)
                        typeMsg = MessageTypeEnum.INFO;
                    else
                        typeMsg = MessageTypeEnum.FAIL;
                }
                lastRead = lastWriteTime;
                logging.Log(resultMsg, typeMsg);
            }
        }

        /**
         * start the handler for listening for a spesific directory.
         */
        public void StartHandleDirectory()
        {
            /**string[] files = Directory.GetFiles(path);
            foreach (string filepath in files)
            {
                string[] args = { filepath };
                    if (isCorrectEnding(filepath))
                        OnCommandRecieved(this, new CommandRecievedEventArgs(1, args, filepath));
            }**/
            lastRead = DateTime.MinValue;
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
        }
        
        /**
         *check if the extension of file is match to one of the png/gif/jpg/bmp
         */
        private bool isCorrectEnding(string filePath)
        {
            string fileEnding = Path.GetExtension(filePath);
            if (fileEnding.Equals(".png", StringComparison.CurrentCultureIgnoreCase)
                        || fileEnding.Equals(".gif", StringComparison.CurrentCultureIgnoreCase)
                        || fileEnding.Equals(".jpg", StringComparison.CurrentCultureIgnoreCase)
                        || fileEnding.Equals(".bmp", StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
            return false;
        }

        /**
         * run the correct command.
         */
        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            this.watcher.EnableRaisingEvents = true; //if you want to listen
            logging.Log("Hendler starts listening to: " + path, MessageTypeEnum.INFO); //add log msg to logging for start listening

            bool result;
            string resultMsg = null;
            MessageTypeEnum typeMsg = new MessageTypeEnum();

            if (e.CommandID == (int)CommandEnum.CloseCommand)   //when the command is close
            { 
                this.watcher.EnableRaisingEvents = false;
                DirectoryCloseEventArgs closeArgs = new DirectoryCloseEventArgs(path, "Handler Closing: " + path);
                DirectoryClose?.Invoke(this, closeArgs);    // and dirctoryClose.Invoke--> for the service
                logging.Log("Handler Closing: " + path, MessageTypeEnum.INFO);  //update the log
            }

            //id dir path is * or my folder, if request == handler.path --> then go, else - return.
            if (!e.RequestDirPath.Equals(path) || e.RequestDirPath.EndsWith("*"))
            {
                resultMsg = controller.ExecuteCommand(e.CommandID, e.Args, out result);
                if (result == false)    // if the command not found
                { 
                    StopHandleDirectory(e.RequestDirPath);
                    typeMsg = MessageTypeEnum.FAIL;
                }
                else
                {
                    typeMsg = MessageTypeEnum.INFO;
                }
            }

            logging.Log(resultMsg, typeMsg);     //update the log
            //watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Changed += new FileSystemEventHandler(OnChanged);
        }

        /**
         *  stop listning to the current path
         */
        public void StopHandleDirectory(string dirPath)
        {
            this.watcher.EnableRaisingEvents = false;
            logging.Log("Stop to handle directory: " + dirPath, MessageTypeEnum.INFO);
        }

        /**
         * when the service is closed, the handler closed too.
         */
        public void OnCloseService(object sender, CommandRecievedEventArgs e)
        {
            ImageServer imageServer = (ImageServer)sender;
            try
            {
                this.watcher.EnableRaisingEvents = false;
                this.watcher.Dispose();
                imageServer.CommandRecieved -= this.OnCommandRecieved;
                this.logging.Log("Handler for path: " + this.path + " was closed.", MessageTypeEnum.INFO);
            }
            catch
            {
                this.logging.Log(this.path + " failed to close the handler.", MessageTypeEnum.WARNING);
            }
        }
    }
}