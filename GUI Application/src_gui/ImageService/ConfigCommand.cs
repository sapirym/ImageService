
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    class ConfigCommand : ICommand
    {
        
            private IImageServiceModal modal;

            public ConfigCommand(IImageServiceModal modal2)
            {
                this.modal = modal2;
            }

            public string Execute(string[] args, out bool result)
            {
                string msg = null;
                try
                {
                string[] arr = new string[5];
                arr[0] = ConfigurationManager.AppSettings.Get("Handler");
                arr[1] = ConfigurationManager.AppSettings.Get("OutputDir");
                arr[2] = ConfigurationManager.AppSettings.Get("SourceName");
                arr[3] = ConfigurationManager.AppSettings.Get("LogName");
                arr[4] = ConfigurationManager.AppSettings.Get("ThumbnailSize");
                //JObject commandLogJson = new JObject();
                string parse = JsonConvert.SerializeObject(arr);
                DataInfo commandSendArgs = new DataInfo(CommandEnum.GetConfigCommand, parse);
                string toJson = JsonConvert.SerializeObject(commandSendArgs);
                result = true;
                return toJson;
            }
                catch (Exception e)
                {
                    result = false;
                    return "failed with transfer file: " + msg;
                }
            }
        }
}
