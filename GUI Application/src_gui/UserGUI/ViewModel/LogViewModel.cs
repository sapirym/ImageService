using ImageService;
using ImageService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserGUI.Communication;
using UserGUI.Model;

namespace UserGUI.View
{
    class LogViewModel : INotifyPropertyChanged
    {
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion
        #region properties
        private LoggingModel model;
        public LoggingModel Model
        {
            get { return this.model; }
            set { this.model = value; }
        }
        private TcpClientConnection tcpClient;
        public ObservableCollection<LogMessege> Logs
        {
            get { return this.model.Logs; }
            set
            {
                this.model.Logs = value;
                NotifyPropertyChanged("Logs");

            }
        }
        #endregion

        /// <summary>
        /// constructor of LogViewModel
        /// </summary>
        public LogViewModel()
        {
            this.model = new LoggingModel();
            model.PropertyChanged +=
            delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged(e.PropertyName);
            };
            this.model.getHistory();
        }
   
    }
}
