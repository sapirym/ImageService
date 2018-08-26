using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UserGUI
{
    public class ClientHandler//: IClientHandler
    {
        /* private static ClientHandler instance = null;

         private ClientHandler(TcpClient client)
         {
             new Task(() =>
             {
                 using (NetworkStream stream = client.GetStream())
                 using (StreamReader reader = new StreamReader(stream))
                 using (StreamWriter writer = new StreamWriter(stream))
                 {
                     string commandLine = reader.ReadLine();
                     Console.WriteLine("Got command: {0}", commandLine);
                     // string result = ExecuteCommand(commandLine, client);
                     //  writer.Write(result);
                 }
                 client.Close();
             }).Start();
         }

         public static ClientHandler getInstance(TcpClient client)
         {
             if (instance != null)
             {
                 throw new InvalidOperationException("Singleton already created - use getinstance()");
             }
             instance = new ClientHandler(client);
             return instance;
         }

         public static ClientHandler getInstance()
         {
             if (instance == null)
                 throw new InvalidOperationException("Singleton not created - use GetInstance(arg1, arg2)");
             return instance;
         }*/
        private static ClientHandler instance;
        private TcpClient client;
        private ClientHandler() {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            TcpClient client = new TcpClient();
            client.Connect(ep);
            Console.WriteLine("You are connected");
            using (NetworkStream stream = client.GetStream())
            using (BinaryReader reader = new BinaryReader(stream))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                // Send data to server
                Console.Write("Please enter a number: ");
                int num = int.Parse(Console.ReadLine());
                writer.Write(num);
                // Get result from server
                int result = reader.ReadInt32();
                Console.WriteLine("Result = {0}", result);
            }
            client.Close();
        }

        public void closeClient()
        {
            client.Close();
        }

        public static ClientHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ClientHandler();
                }
                return instance;
            }
        }

    }
}
