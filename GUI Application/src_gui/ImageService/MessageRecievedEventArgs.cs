using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    public class MessageRecievedEventArgs : EventArgs
    {
        /**
         * constructor for msg eve args.
         */
        public MessageRecievedEventArgs(MessageTypeEnum status, string message)
        {
            Status = status;
            Message = message;
        }
        public MessageTypeEnum Status { get; set; }
        public string Message { get; set; }

        public static EventLogEntryType toEventLogEntryType(MessageTypeEnum type)
        {
            switch ((int)type)
            {
                case 1: return EventLogEntryType.Error;
                case 2: return EventLogEntryType.Warning;
                case 4: return EventLogEntryType.Information;
                default: return EventLogEntryType.Information;
            }
        }

        public static MessageTypeEnum toMessageTypeEnum(EventLogEntryType type)
        {
            switch ((int)type)
            {
                case 1: return MessageTypeEnum.FAIL;
                case 2: return MessageTypeEnum.WARNING;
                case 4: return MessageTypeEnum.INFO;
                default: return MessageTypeEnum.INFO;
            }
        }

    }
}
