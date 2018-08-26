using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ImageService;
using UserGUI.Communication;
using System.Windows;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Diagnostics;

namespace UserGUI.Model
{
    class LoggingModel: ILoggingModel
    {
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion
        #region properties
        public TcpClientConnection clientConnection { get; set; }
        private ObservableCollection<LogMessege> logs;
        public ObservableCollection<LogMessege> Logs
        {
            get { return this.logs; }
            set
            {
                this.logs = value;
                OnPropertyChanged("logs list");
            }
        }
        #endregion
        /// <summary>
        /// constructor of LoggingModel
        /// </summary>
        public LoggingModel()
        {
            logs = new ObservableCollection<LogMessege>();
            this.clientConnection = TcpClientConnection.Instance;
            BindingOperations.EnableCollectionSynchronization(logs, new Object());
            clientConnection.MessageReceived += messageRecieved;
        }
        
        /// <summary>
        /// the function send request from the server to the history log
        /// </summary>
        public void getHistory()
        {
            DataInfo msg = new DataInfo(CommandEnum.LogCommand, null);
            clientConnection.WriteToServer(msg.toJson());
        }

        /// <summary>
        ///event- according to the id, do action
        ///if the id is newLogEntryCommand- add new log msg to the list
        ///if the id is LogCommand - add the history
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="msg"></param>
        /// 
        public void messageRecieved(object sender, DataInfo info)
        {
            if (info.Id == CommandEnum.newLogEntryCommand)
            {
                MessageRecievedEventArgs recLog = JsonConvert.
                       DeserializeObject<MessageRecievedEventArgs>(info.Args);
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    logs.Insert(0, new LogMessege(recLog.Status, recLog.Message)); 
                }));

            }else if (info.Id == CommandEnum.LogCommand)
            {
                try
                { 
                    List<MessageRecievedEventArgs> recLog = JsonConvert.
                        DeserializeObject<List<MessageRecievedEventArgs>>(info.Args);
                    foreach (MessageRecievedEventArgs entry in recLog)
                    {
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            logs.Insert(0, new LogMessege(entry.Status, entry.Message));
                        }));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        
    }

}

