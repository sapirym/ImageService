using ImageService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    public interface ILoggingService
    {
        event EventHandler<MessageRecievedEventArgs> MessageRecieved; //send to eventLog the message we want to update
        void Log(string message, MessageTypeEnum type); // Logging the Message
        List<MessageRecievedEventArgs> getHistoryLog();
        void cleanLog();
        void newLog(object sender, MessageRecievedEventArgs e);
    }
}
