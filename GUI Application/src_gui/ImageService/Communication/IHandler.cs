
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Communication
{
    public delegate void NewClientDelegate(TcpClient client);
    interface IHandler
    {
        event EventHandler<CommandRecievedEventArgs> CommandRecieved;

        void serverHandle(TcpClient client);
        void SendMessageToAllClients(string message);


    }
}
