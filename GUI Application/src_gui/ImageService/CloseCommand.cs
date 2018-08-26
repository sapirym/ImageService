
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    class CloseCommand : ICommand
    {
        public event EventHandler<DirectoryCloseEventArgs> Closed;

        public string Execute(string[] args, out bool result)
        {
            DirectoryCloseEventArgs e = new DirectoryCloseEventArgs(args[0], "Closing handler");
            // invoke event notifying that a handler needs to close
            Closed?.Invoke(this, e);
            StringBuilder sb = new StringBuilder();
            string[] handlers = ConfigurationManager.AppSettings.Get("Handler").Split(';');
            foreach (string handler in handlers)
            {
                if (string.Compare(args[0], handler) != 0)
                {
                    sb.Append(handler);
                    sb.Append(";");
                }
            }
            ConfigurationManager.AppSettings.Set("Handler", sb.ToString());
            result = true;
            //return "Executed Close Command with arguments: " + args[0];
            
            MessageRecievedEventArgs info = new MessageRecievedEventArgs(MessageTypeEnum.WARNING, args[0]);
            string parse = JsonConvert.SerializeObject(info);
            DataInfo commandSendArgs = new DataInfo(CommandEnum.CloseCommand, parse);
            string toJson = JsonConvert.SerializeObject(commandSendArgs);
            return toJson;
        }
    }
}
