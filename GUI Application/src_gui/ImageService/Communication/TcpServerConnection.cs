using ImageService.Communication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using ImageService;
using System.Threading;
using Newtonsoft.Json;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace CommunicationImageService
{
    class TcpServerConnection
    {
        private static Mutex mutex = new Mutex();
        private IHandler sh;
        private List<TcpClient> clients;
        private bool closeCommunication;
        private TcpListener m_listener;
        public int port { get; set; }
        public string ip { get; set; }
        public TcpListener listener { get; set; }
        public IHandler Sh
        {
            get {
                return this.sh;
            }
            set {
                this.sh = value;
            }
        }

        private bool CloseCommunication
        {
            get {
                return this.closeCommunication;
            }
            set {
                this.closeCommunication = value;
            }
        }

        private ILoggingService m_logging;

        public TcpServerConnection(IImageController imageController, ILoggingService logging)
        {
            this.port =  int.Parse(ConfigurationManager.AppSettings["Port"]);
            this.ip = ConfigurationManager.AppSettings["Ip"];
            this.Sh = new ServerHandler(imageController,logging);
            this.clients = new List<TcpClient>();
            this.CloseCommunication = false;
            this.m_logging = logging;
        }

        private void removeClient(TcpClient client)
        {
            client.Close();
            clients.Remove(client);
        }

        public void sendMsgToAll(string msg)
        {
            this.sh.SendMessageToAllClients(msg);
        }

        public void Start()
        {
            this.closeCommunication = false;
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(this.ip), this.port);
            this.m_listener = new TcpListener(ep);

            this.m_listener.Start();
            this.m_logging.Log("Waiting for connections...", MessageTypeEnum.INFO);
            Task task = new Task(() => {
                while (!closeCommunication)
                {
                    try
                    {
                        // accept client
                        TcpClient client = this.m_listener.AcceptTcpClient();
                        clients.Add(client);
                        this.m_logging.Log("Client Connected", MessageTypeEnum.INFO);
                        this.sh.serverHandle(client);
                    }
                    catch (SocketException e)
                    {
                        this.m_logging.Log(e.Message, MessageTypeEnum.FAIL);
                    }
                }
                this.m_logging.Log("Server stopped", MessageTypeEnum.INFO);
            });
            task.Start();
        }

        public void Stop()
        {
            DataInfo data = new DataInfo(CommandEnum.CloseAll, "close Server");
            this.sendMsgToAll(data.toJson());
            this.CloseCommunication = true;
            listener.Stop();
            foreach(TcpClient tp in this.clients)
            {
                tp.Close();
                clients.Clear();
            }
        }


    }
}
