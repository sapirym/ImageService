using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserGUI.Communication;

namespace UserGUI.Model
{
    class MainWindowModel: INotifyPropertyChanged
    {
        private TcpClientConnection tcpClient;
        public TcpClientConnection TcpClient{
            get {
                return tcpClient;
            }
            set {
                tcpClient = value;
            }
        }

        private bool connect;
        public bool Connect
        {
            get { return this.connect; }
            set
            {
                this.connect = value;
                NotifyPropertyChanged("Connect");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        

        public MainWindowModel()
        {
            this.tcpClient = TcpClientConnection.Instance;
            this.connect = tcpClient.Connect;
        }
    }
}

