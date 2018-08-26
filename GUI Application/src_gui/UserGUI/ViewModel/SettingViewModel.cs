using ImageService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserGUI.Model;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Net.Sockets;
using UserGUI.Communication;

namespace UserGUI.View
{
    class SettingViewModel : INotifyPropertyChanged
    {
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        private void newPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var command = this.removeCommand as DelegateCommand<object>;
            command?.RaiseCanExecuteChanged();

        }
        #endregion

        public event SendToService sendMsg;
        private NetworkStream stream;
        private BinaryWriter writer;
        private BinaryReader reader;
        public System.Windows.Input.ICommand removeCommand { get; private set; }
        
        #region propertiey
        private SettingModel model;
        public SettingModel Model
        {
            get { return this.model; }
            set
            {
                this.model = value;
                NotifyPropertyChanged("Model");
            }
        }
        public string ThumbnailSize
        {
            get { return this.model.ThumbnailSize; }
        }
        public string LogName
        {
            get { return this.model.LogName; }
            set
            {
                this.model.ThumbnailSize = value;
                NotifyPropertyChanged("LogName");
            }
        }
        public string SourceName
        {
            get { return this.model.SourceName; }
            set
            {
                this.model.ThumbnailSize = value;
                NotifyPropertyChanged("SourceName");
            }
        }
        public string OutputDir
        {
            get { return this.model.OutputDir; }
            set
            {
                this.model.ThumbnailSize = value;
                NotifyPropertyChanged("OutputDir");
            }
        }
        public ObservableCollection<string> Handler
        {
            get { return this.model.Handler; }
            set
            {
                this.model.Handler = value;
                NotifyPropertyChanged("Handler");

            }
        }
        private string chooseItem;
        public string ChooseItem
        {
            get { return this.chooseItem; }
            set
            {
                this.chooseItem = value;
                NotifyPropertyChanged("ChooseItem");

               
            }
        }
        #endregion

        /// <summary>
        /// constructor of SettingViewModel
        /// </summary>
        public SettingViewModel()
        {
            this.model = new SettingModel();
            this.removeCommand = new DelegateCommand<object>(this.OnRemove, this.canRemove);
            model.PropertyChanged +=
            delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged(e.PropertyName);
            };
            this.PropertyChanged += newPropertyChanged;
            this.model.getFirstConfi();
            this.chooseItem = null;
        }

        /// <summary>
        /// when we click on the button, this function happen.
        /// in our case- the choose itrm remove from the list
        /// </summary>
        /// <param name="obj"></param>
        private void OnRemove(object obj)
        {
            if (chooseItem != null)
            {
                DataInfo remove = new DataInfo(CommandEnum.CloseCommand, this.chooseItem);
                TcpClientConnection client = TcpClientConnection.Instance;
                client.WriteToServer(remove.toJson());
                this.chooseItem = null;
            }
        }

        /// <summary>
        /// the function decide when we can click to the button
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool canRemove(object obj)
        {
            return (!(string.IsNullOrEmpty(this.chooseItem)));
        }

        }
}
