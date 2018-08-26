using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;
            // The event that notifies about a new Command being recieved
        public event EventHandler<CommandRecievedEventArgs> CloseTheService;
        #endregion

        /**
         * constructor
         */
        public ImageServer(IImageController m, ILoggingService m_logging1)
        {
            this.m_controller = m;
            this.m_logging = m_logging1;
        }

        /**
         * run the service
         * */
        public void Start()
        {
            string outPutDirPath = ConfigurationManager.AppSettings["Handler"];
            string[] arrPath = outPutDirPath.Split(';');
            foreach (string Item in arrPath)
            {
                 createHandler(Item);
            }
        }

        /**
         * stop the service.
         */
        public void Stop()
        {
            CommandRecieved?.Invoke(this, new CommandRecievedEventArgs(2, null, "*"));
        }

        /**
         * the function create handler to specific path
         */
        public void createHandler(string path)
        {
            IDirectoryHandler h = new DirectoyHandler(path, m_controller, m_logging);
            CommandRecieved += h.OnCommandRecieved;//directory
            this.CloseTheService += h.OnCloseService;
            h.DirectoryClose += closeServer;
            m_logging.Log("Created handler: " + path, MessageTypeEnum.INFO);
            h.StartHandleDirectory();
        }

        /**
         * the function close server after call to the close
         */
        public void closeServer(object sender, DirectoryCloseEventArgs e)
        {
            IDirectoryHandler h = (IDirectoryHandler)sender;
            CommandRecieved -= h.OnCommandRecieved;
            m_logging.Log("close service: " + e.Message, MessageTypeEnum.INFO);
        }

        /**
         * invoke an event for closing.
         */
        public void CloseService()
        {
            CloseTheService?.Invoke(this, null);
        }
    }
}
