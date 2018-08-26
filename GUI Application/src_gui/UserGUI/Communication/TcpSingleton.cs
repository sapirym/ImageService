using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserGUI.Communication
{
    class TcpSingleton
    {
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        private static TcpSingleton s_instance = null;
        private bool m_isConnected;
        public bool IsConnected
        {
            get { return m_isConnected; }
            set
            {
                m_isConnected = value;
                NotifyPropertyChanged("is connected");
            }
        }

        public TcpSingleton Instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = new TcpSingleton();
                }
                return s_instance;
            }
        }

        public TcpClientConnection Connection { get; set; }

        public TcpSingleton()
        {
        }
    }
}
