using ImageService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserGUI.Model
{
    class LogMessege : INotifyPropertyChanged
    {
        #region properties
        private MessageTypeEnum type;
        public MessageTypeEnum Type
        {
            get { return this.type; }
            set { this.type = value;
                onPropertyChanged("Type");
            }
        }
        private string message;

        public string Message
        {
            get { return this.message; }
            set
            {
                this.message = value;
                onPropertyChanged("Message");

            }
        }
        #endregion
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        public void onPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
        /// <summary>
        /// constructor of LogMessege
        /// </summary>
        /// <param name="type"></param>
        /// <param name="msg"></param>
        public LogMessege(MessageTypeEnum type, string msg)
        {
            this.type = type;
            this.message = msg;
        }
        
        

        
    }
}
