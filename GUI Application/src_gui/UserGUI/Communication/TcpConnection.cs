using ImageService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserGUI.Communication;

namespace UserGUI.Communication
{
    interface TcpConnection
    {
        void Start();
        void Stop();
    }
}
