using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ImageService;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading;


namespace UserGUI.Communication
{
    public class ClientHandler: IClientHandler
    {
        private static Mutex mutex = new Mutex();
        private NetworkStream stream;
        public NetworkStream Stream { get { return this.stream; } set { this.stream = value; } }
        private BinaryWriter writer;
        private BinaryReader reader;
        public event UpdateUsers updateAll;
        public event UpdateUsers updateAllSet;

        bool stop;
        public TcpClient client;
        public TcpClient Client
        {
            get { return this.client; }
            set { this.client = value; }
        }
        //private NetworkStream stream ;
        //NetworkStream IClientHandler.Stream { get { return this.stream; } set { this.stream = value; } }


        public ClientHandler()
        {
            this.stop = false;
            this.Client = new TcpClient();
            Console.WriteLine("in client handler");
            
        }
        private class MyBinaryReader : BinaryReader
        {
            public MyBinaryReader(Stream s) : base(s) { }
            public string ReadCString()
            {
                int size = this.ReadByte();
                if (size == 0) return string.Empty;
                return new string(this.ReadChars(size));
            }
        }

        public void receive()
        {
            Console.WriteLine("in recieve");
            new Task(() =>
            {
                Console.WriteLine("in Task");
                try
                {
                    Console.WriteLine("in try");
                    //using (NetworkStream stream = client.GetStream())
                    Stream = Client.GetStream();
                    using (BinaryReader reader = new BinaryReader(Stream))
                    {
                        while (!stop)
                        {
                            Console.WriteLine("in while & before client got msg");
                            string command = reader.ReadString();

                            Console.WriteLine("data recived: "+ command);
                            if (command != null)
                            {
                                DataInfo msg1 = JsonConvert.DeserializeObject<DataInfo>(command);
                                Console.WriteLine("got msg: " + command);

                                if (false||msg1.Id==CommandEnum.CloseCommand)//command.Contains("close Server."))
                                {
                                    this.stop = false;
                                    stream.Close();
                                    reader.Close();
                                    client.Close();
                                }
                                else if(msg1.Id == CommandEnum.LogCommand)
                                {
                                    //DataInfo msg = JsonConvert.DeserializeObject<DataInfo>(command);
                                    Console.WriteLine("in else! clienthandler..." + msg1.Args);
                                    this.updateAll?.Invoke(msg1);
                                    Thread.Sleep(700);
                                }
                            }
                        }
                    }
                } catch (Exception e)
                {
                    Console.WriteLine("cant read from server" + e);
                }
            }).Start();
        }

        public void invokeClient(DataInfo msg1)
        {
            this.updateAll?.Invoke(msg1);
        }

        public void invokeClientSet(DataInfo msg1)
        {
            this.updateAllSet?.Invoke(msg1);
        }

        public void sendMsg(DataInfo e)
        {
         //   new Task(() => {
                // using (NetworkStream stream = this.client.GetStream())
                //using (BinaryWriter writer = new BinaryWriter(stream))
                try
                {
                if (Client.Connected == true)
                {
                    Stream = this.Client.GetStream();
                    writer = new BinaryWriter(Stream);

                    {
                        mutex.WaitOne();
                        writer.Write(e.toJson());
                        mutex.ReleaseMutex();
                    }
                }
                }catch(Exception ex)
                {
                    Console.WriteLine(ex.Data);
                }
           // }).Start();
        }



    }
    }
