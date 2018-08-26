using ImageService;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace ImageService.Communication
{
    class ServerHandler: IHandler
    {
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;
        private Mutex mutex = new Mutex();
        private List<TcpClient> m_clientList = new List<TcpClient>();
    
        public IImageController ImageController { get; set; }
        public ILoggingService logging { get; set; }
        /**
         * constructor for server handler.
         */
        public ServerHandler(IImageController imageController, ILoggingService logging)
        {
            this.ImageController = imageController;
            this.logging = logging;
        }

        /**
         * function that handle the server
         */
        public void serverHandle(TcpClient client)
        {
            new Task(() =>
            {
                NetworkStream stream = client.GetStream();
                BinaryReader reader = new BinaryReader(stream);
                BinaryWriter writer = new BinaryWriter(stream);
                this.m_clientList.Add(client);
                bool clientConnect = true;
                try
                {
                    while (clientConnect)
                    {
                        string commandLine = reader.ReadString();
                        DataInfo info = JsonConvert.DeserializeObject<DataInfo>(commandLine);
                        if (info != null)
                        {
                           /* if (info.Id == CommandEnum.GetConfigCommand)
                            {
                                string[] arr = new string[5];
                                arr[0] = ConfigurationManager.AppSettings.Get("Handler");
                                arr[1] = ConfigurationManager.AppSettings.Get("OutputDir");
                                arr[2] = ConfigurationManager.AppSettings.Get("SourceName");
                                arr[3] = ConfigurationManager.AppSettings.Get("LogName");
                                arr[4] = ConfigurationManager.AppSettings.Get("ThumbnailSize");
                                JObject commandLogJson = new JObject();
                                string parse = JsonConvert.SerializeObject(arr);
                                DataInfo commandSendArgs = new DataInfo(CommandEnum.GetConfigCommand, parse);
                                string toJson = JsonConvert.SerializeObject(commandSendArgs);
                                mutex.WaitOne();
                                writer.Write(toJson);
                                
                                this.logging.Log("suceeded on config: " + info.Args, MessageTypeEnum.INFO);
                                mutex.ReleaseMutex();
                            }*/
                           /* if (info.Id == CommandEnum.CloseCommand)
                            {
                                string[] array = ConfigurationManager.AppSettings["Handler"].Split(';');
                                foreach (string s in array)
                                {
                                    if (s.Equals(info.Args))
                                    {
                                        array = array.Where(val => val != info.Args).ToArray();
                                        string joined = String.Join(";", array);
                                        ConfigurationManager.AppSettings["Handler"] = joined;
                                         DataInfo data = new DataInfo(CommandEnum.CloseCommand,
                                            info.Args);
                                        this.SendMessageToAllClients(data.toJson());
                                    }
                                }
                                this.logging.Log("close the command: " + info.Args, MessageTypeEnum.WARNING);
                            }
                                
                            else*/
                            
                                bool outResult;
                                string[] arr = new string[1];
                                arr[0] = info.Args;
                                string result = this.ImageController.ExecuteCommand((int)info.Id, arr, out outResult);
                                if (outResult){
                                this.logging.Log("succeeded execute command from client: " + info.Id, MessageTypeEnum.INFO);
                                if (info.Id == CommandEnum.CloseCommand)
                                    {
                                        this.SendMessageToAllClients(result);
                                    this.logging.Log("close the command: " + info.Args, MessageTypeEnum.WARNING);

                                }
                                else {
                                        try {
                                            mutex.WaitOne();
                                            writer.Write(result);
                                            mutex.ReleaseMutex();
                                        } catch {
                                            m_clientList.Remove(client);
                                        }
                                    }
                                } else {
                                    this.logging.Log("failed on execute command from client", MessageTypeEnum.FAIL);
                                }
                            }   
                        }
                    
                }
                catch (Exception e)
                {
                    clientConnect = false;
                    client.Close();
                    this.m_clientList.Remove(client);
                    return;
                }
            }).Start();
        }

        /**
               * send to all client msg.
               */
        public void SendMessageToAllClients(string message)
        {
            new Task(() =>
            {
               //  send message to all connected clients
                foreach (TcpClient clientInfo in this.m_clientList)
                {
                    try
                    {
                        NetworkStream stream = clientInfo.GetStream();
                        BinaryReader reader = new BinaryReader(stream);
                        BinaryWriter writer = new BinaryWriter(stream);
                        mutex.WaitOne();
                        writer.Write(message);
                        mutex.ReleaseMutex();
                    }
                    catch (Exception e)
                    {
                        this.m_clientList.Remove(clientInfo);
                        clientInfo.Close();
                    }
                }
            }).Start();
        }
    }
}