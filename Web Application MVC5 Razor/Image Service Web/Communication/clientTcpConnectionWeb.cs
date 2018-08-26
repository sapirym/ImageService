using ImageService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ImageServiceWeb.Communication
{
    public class clientTcpConnectionWeb
    {
       
        public event EventHandler<DataInfo> MessageReceived;

        private static clientTcpConnectionWeb instance;
        private Mutex mutex = new Mutex();

        private TcpClient client;
        public TcpClient Client
        {
            get
            {
                return this.client;
            }
        }

        private NetworkStream stream;
        private BinaryReader reader;
        private BinaryWriter writer;

        private bool endCommunication;
        private bool isConnect;
        public bool Connect { get { return this.isConnect; } }

        public static clientTcpConnectionWeb Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new clientTcpConnectionWeb();
                }
                return instance;
            }
        }

        /// <summary>
        /// constructor of TcpClientConnection
        /// </summary>
        private clientTcpConnectionWeb()
        {
            this.isConnect = StartCommunication();
        }

        public string IsServiceConnect()
        {
            if (isConnect)
            {
                return "Connect";
            }else
            {
                return "Not Connect";
            }
        }

        /// <summary>
        /// start communication with server
        /// </summary>
        /// <returns>true if connect</returns>
        private bool StartCommunication()
        {
            try
            {
                // set
                int port = 8000;// int.Parse(ConfigurationManager.AppSettings["Port"]);
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);//ConfigurationManager.AppSettings["Ip"])
                this.client = new TcpClient();
                // connect
                client.Connect(ep);
                this.stream = client.GetStream();
                this.reader = new BinaryReader(stream);
                this.writer = new BinaryWriter(stream);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                isConnect = false;
                return false;
            }
            isConnect = true;
            endCommunication = false;
            ReadFromServer();
            return true;
        }

        /// <summary>
        /// write string to the server
        /// </summary>
        /// <param name="str"></param>
        public void WriteToServer(string str)
        {
            if (this.Connect)
            {
                try
                {
                    writer.Write(str);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// read from server all the time, and notify when new message sent
        /// </summary>
        public void ReadFromServer()
        {
            new Task(() =>
            {
                while (!endCommunication)
                {
                    try
                    {
                        mutex.WaitOne();
                        string message = reader.ReadString();
                        mutex.ReleaseMutex();
                        if (message != null)
                        {
                            DataInfo info = JsonConvert.DeserializeObject<DataInfo>(message);
                            /*if (info.Id == CommandEnum.CloseAll)
                            {
                                CloseCommunication();
                                break;
                            }*/
                            this.MessageReceived?.Invoke(this, info);
                            Thread.Sleep(1000);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Data);
                        break;
                    }
                }
            }).Start();
        }

        /// <summary>
        /// close the communication
        /// </summary>
        public void CloseCommunication()
        {
            try
            {
                isConnect = false;
                endCommunication = true;
                this.writer.Close();
                this.reader.Close();
                this.stream.Close();
                this.client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}