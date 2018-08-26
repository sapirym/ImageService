
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    public class LoggingService : ILoggingService
    {
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        private List<MessageRecievedEventArgs> history;//List<Tuple<MessageTypeEnum, string>> history;
        /**
         * add log msg to log.
         */
         public LoggingService()
        {
            history= new List<MessageRecievedEventArgs>();
        }
        public void Log(string message, MessageTypeEnum type)
        {
            history.Add(new MessageRecievedEventArgs(type, message));
            MessageRecieved?.Invoke(this, new MessageRecievedEventArgs(type,message));
        }

        public void newLog(object sender, MessageRecievedEventArgs e)
        {
            this.history.Add(e);
        }
        public List<MessageRecievedEventArgs> getHistoryLog()
        {
            return history;
        }
        public void cleanLog()
        {
            history.Clear();
        }
    }
}
