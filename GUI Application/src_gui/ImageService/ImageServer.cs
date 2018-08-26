
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using CommunicationImageService;
using ImageService.Communication;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ImageService
{
    public class ImageServer
    {
        public const int CLOSE_SERVER = 4;
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        private int port;
        private TcpListener listener;
        private TcpServerConnection tcpServer;
        private List<IDirectoryHandler> handlers;
       /* public TcpServerConnection TcpServer
        {
            get { return this.tcpServer; }
            set { this.tcpServer = value; }
        }*/

        #endregion


        #region Properties
        //public event EventHandler<DataInfo> sendCommand;
        //public event EventHandler<DataInfo> updateClients;
        // The event that notifies about a new Command being recieved
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;
        //public event EventHandler<CommandRecievedEventArgs> DataRecieved;
        public event EventHandler<CommandRecievedEventArgs> CloseTheService;

        //public event EventHandler<DataInfo> OnNewClient;
        //Dictionary<int, CommandAction> m_action;
        #endregion

        /**
         * constructor
         */
        public ImageServer(IImageController m, ILoggingService m_logging1,int port)
        {
            this.m_controller = m;
            this.m_logging = m_logging1;
            this.port = 8001;
            this.tcpServer = new TcpServerConnection(this.m_controller, m_logging);
            this.tcpServer.Sh.CommandRecieved += sendCmd;
            // this.OnNewClient += this.tcpServer.onNewClient;
            handlers = new List<IDirectoryHandler>();
        }

        /**
         * run the service
         * */
        public void Start()
        {
            tcpServer.Start();
            string outPutDirPath = ConfigurationManager.AppSettings["Handler"];
            string[] arrPath = outPutDirPath.Split(';');
            foreach (string Item in arrPath)
            {
                 createHandler(Item);
            }
        }


        public void sendClients(MessageRecievedEventArgs e)
        {
            DataInfo cmdMsg =
                new DataInfo(CommandEnum.newLogEntryCommand, JsonConvert.SerializeObject(e));
            this.tcpServer.sendMsgToAll(cmdMsg.toJson());
        }

        public void updareClientsHandlers(MessageRecievedEventArgs e)
        {
            DataInfo cmdMsg =
                new DataInfo(CommandEnum.CloseCommand, JsonConvert.SerializeObject(e));
            this.tcpServer.sendMsgToAll(cmdMsg.toJson());
        }

        public void sendLog(string msg, MessageTypeEnum status)
        {
            MessageRecievedEventArgs msg1 = new MessageRecievedEventArgs(status, msg);
            string message;
           /* if ((message = JsonConvert.SerializeObject(msg1)) == null)
            {
                Debug.WriteLine("Json not work");
                return;
            }*/
            DataInfo cmdMsg = 
                new DataInfo(CommandEnum.newLogEntryCommand, JsonConvert.SerializeObject(msg1));

            Debug.WriteLine("in sendlog!!");
            /*List<MessageRecievedEventArgs> args = m_logging.getHistoryLog();
            string argsJson = JsonConvert.SerializeObject(args);
            DataInfo inf = new DataInfo(CommandEnum.LogCommand, argsJson);
            string infJson = JsonConvert.SerializeObject(inf);
            //this.m_logging.Log("in send log", MessageTypeEnum.INFO);
            this.tcpServer.sendMsgToAll(infJson);*/
            //string infJson = JsonConvert.SerializeObject(cmdMsg);
            this.tcpServer.sendMsgToAll(cmdMsg.toJson()); 
        }

        public void sendCmd(object sender, CommandRecievedEventArgs data)
        {
            CommandRecieved?.Invoke(this, data);
        }

        /**
         * stop the service.
         */
        public void Stop(){
            CommandRecieved?.Invoke(this, new CommandRecievedEventArgs(CLOSE_SERVER, null, "*"));
            tcpServer.Stop();
        }

        /**
         * the function create handler to specific path
         */
        public void createHandler(string path){
            IDirectoryHandler h = new DirectoyHandler(path, m_controller, m_logging);
            handlers.Add(h);
            CommandRecieved += h.OnCommandRecieved;//directory
            this.CloseTheService += h.OnCloseService;
            h.DirectoryClose += closeService;
            m_logging.Log("Created handler: " + path, MessageTypeEnum.INFO);
            h.StartHandleDirectory();
        }

        /**
         * the function close server after call to the close
         */
        public void closeService(object sender, DirectoryCloseEventArgs e)
        {
            IDirectoryHandler h = (IDirectoryHandler)sender;
            CommandRecieved -= h.OnCommandRecieved;
            m_logging.Log("close handler: " + e.Message, MessageTypeEnum.INFO);
        }

        public void CloseHandler(object sender, DirectoryCloseEventArgs e)
        {
            int flag = 0;
            foreach (IDirectoryHandler handler in handlers)
            {
                if (e.DirectoryPath.Equals("*") || handler.getPath().Equals(e.DirectoryPath))
                {
                    flag = 1;
                    this.CommandRecieved -= handler.OnCommandRecieved;
  //                  this.m_logging.Log("Closing handler for " + e.DirectoryPath, MessageTypeEnum.INFO);
                    handler.DirectoryClose -= CloseHandler;
                    handler.StopHandleDirectory(e.DirectoryPath);
                   this.m_logging.Log("Closed handler for " + e.DirectoryPath, MessageTypeEnum.INFO);
                    string path = e.DirectoryPath;
                    string[] args = { path };

                    //DataInfo info = new DataInfo(CommandEnum.CloseCommand, JsonConvert.SerializeObject(args));
                    MessageRecievedEventArgs info = new MessageRecievedEventArgs(MessageTypeEnum.WARNING, path);
                    updareClientsHandlers(info);
                }
            }
            if (flag==0)
            {
                MessageRecievedEventArgs info = new MessageRecievedEventArgs(MessageTypeEnum.FAIL, null);
                //sen.Invoke(this, info);
                sendClients(info);
            }
        }

        public void CloseServer()
        {
            CommandRecievedEventArgs commandRecEventArgs = new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, null, null);
            CommandRecieved?.Invoke(this, commandRecEventArgs);
            try
            {
                listener.Stop();
            }
            catch (Exception e)
            {
                m_logging.Log(e.Message, MessageTypeEnum.FAIL);
            }
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
