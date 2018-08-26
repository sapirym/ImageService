using System;
using System.Configuration;

namespace ImageService
{
    internal class CloseAllHendlersCommand : ICommand
    {
        public string Execute(string[] args, out bool result)
        {

           // TcpClient tcp = JsonConvert.DeserializeObject<TcpClient>(info.Args);
           // this.NewClient?.Invoke(tcp);

            string handlers = ConfigurationManager.AppSettings["Handler"];
            if (handlers.Contains(args[0]))
            {
                //    if (handlers.Contains(args[0]))
                //  {
                //    handlers.
                //}
                int handlerPos = handlers.IndexOf(args[0]);
                string afterRemove = handlers.Remove(handlerPos);
                ConfigurationManager.AppSettings["Handler"] = afterRemove;
                //save
                result = true;
                return args[0] + " moved From the handlers list.";
            }
            else
            {
                result = false;
                return "the handler doesn't exist. Can't remove it.";
            }
        }
    }
}