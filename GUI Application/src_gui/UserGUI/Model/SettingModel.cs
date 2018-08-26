using ImageService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using UserGUI.Communication;
using System.Windows;
namespace UserGUI.Model
{
    class SettingModel : ISettingModel
    {
        private TcpClientConnection clientConnection;
        #region members
        private string thumbnailSize;
        private string logName;
        private string sourceName;
        private string outputDir;
        public ObservableCollection<string> handler;
        private string chooseHendler;

        #endregion
        #region properties
        public string ThumbnailSize
        {
            get { return this.thumbnailSize; }
            set
            {
                this.thumbnailSize = value;
                NotifyPropertyChanged("ThumbnailSize");
            }
        }
        public string LogName
        {
            get { return this.logName; }
            set
            {
                this.logName = value;
                NotifyPropertyChanged("LogName");
            }
        }
        public string SourceName
        {
            get { return this.sourceName; }
            set
            {
                this.sourceName = value;
                NotifyPropertyChanged("SourceName");

            }
        }
        public string OutputDir
        {
            get { return this.outputDir; }
            set
            {
                this.outputDir = value;
                NotifyPropertyChanged("OutputDir");
            }
        }
        public ObservableCollection<string> Handler
        {
            get { return this.handler; }
            set
            {
                this.handler = value;
                NotifyPropertyChanged("handler list");
            }
        }
        public string ChooseHendler
        {
            get { return this.chooseHendler; }
            set
            {
                this.chooseHendler = value;
                NotifyPropertyChanged("ChooseHendler");
            }
        }
        #endregion

        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        }
        #endregion

        /// <summary>
        /// constructor of SettingModel
        /// </summary>
        public SettingModel()
        {
            handler = new ObservableCollection<string>();
            clientConnection = TcpClientConnection.Instance;
            Thread.Sleep(1000);
            BindingOperations.EnableCollectionSynchronization(handler, new Object());
            clientConnection.MessageReceived += onGetConfig;
        }

        /// <summary>
        /// the function send request from the server to the set config
        /// </summary>               
        public void getFirstConfi()
        {
            DataInfo msg = new DataInfo(CommandEnum.GetConfigCommand, null);
            clientConnection.WriteToServer(msg.toJson());
        }

        /// <summary>
        /// event- according to the id, do action
        /// if the id is CloseCommand- remove the path string from the hansler's list
        /// if the id is GetConfigCommand - get and update the propeties
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="msg"></param>
        public void onGetConfig(object sender, DataInfo msg)
        {
            if (msg.Id == CommandEnum.CloseCommand)
            {
                MessageRecievedEventArgs m = JsonConvert.DeserializeObject<MessageRecievedEventArgs>(msg.Args);
                if (m.Status != MessageTypeEnum.FAIL && m.Message != null)
                {
                    if (Handler != null)
                    {
                        if(Handler.Any(p => p == m.Message))
                        {
                            Application.Current.Dispatcher.Invoke(new Action(() => { Handler.Remove(m.Message); }));
                            NotifyPropertyChanged("handler list");
                        }
                    }
                }
            } 
            else if (msg.Id == CommandEnum.GetConfigCommand) {
                string[] arr = JsonConvert.DeserializeObject<string[]>(msg.Args);
                this.outputDir = arr[1];
                this.sourceName = arr[2];
                this.logName = arr[3];
                this.thumbnailSize = (string)arr[4];
                string hanslersToSplit = arr[0];
                string[] temp = hanslersToSplit.Split(';');
                foreach (string item in temp)
                    this.handler.Add(item);
            }
        }
        
    }
}
