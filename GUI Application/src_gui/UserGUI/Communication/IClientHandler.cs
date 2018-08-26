using ImageService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UserGUI.Communication
{
    public delegate void UpdateUsers(DataInfo msg);
    interface IClientHandler
    {
        NetworkStream Stream { get; set; }
        event UpdateUsers updateAll;
         event UpdateUsers updateAllSet;
        TcpClient Client { get; set; }
        void invokeClient(DataInfo msg1);
        void invokeClientSet(DataInfo msg1);
        void receive();
        void sendMsg(DataInfo e);

    }
}
