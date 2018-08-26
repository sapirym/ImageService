using ImageService;
using ImageServiceWeb.Communication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
//using UserGUI.Communication;
namespace ImageServiceWeb.Models
{
    public class LogModel
    {
        private bool ifLogUpdate;
        /// <summary>
        /// constructor of log model
        /// </summary>
        public LogModel()
        {
            clientTcpConnectionWeb singelton = clientTcpConnectionWeb.Instance;
            string serviceConnect = singelton.IsServiceConnect();
            ifLogUpdate = false;
            if (serviceConnect.Equals("Connect"))
            {
                Logs = new List<LogWeb>();
                singelton.MessageReceived += getlogs;
                DataInfo msg = new DataInfo(CommandEnum.LogCommand, null);
                singelton.WriteToServer(msg.toJson());
                while (!ifLogUpdate)
                {
                    Thread.Sleep(100);
                }
            }
        }

        /// <summary>
        /// get logs function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="msg"></param>
        private void getlogs(object sender, DataInfo msg)
        {
            if (msg.Id == CommandEnum.LogCommand)
            {
                try
                {
                    List<MessageRecievedEventArgs> recLog = JsonConvert.
                        DeserializeObject<List<MessageRecievedEventArgs>>(msg.Args);
                    Logs.Clear();
                    foreach (MessageRecievedEventArgs entry in recLog)
                    {
                        LogWeb logWeb = new LogWeb()
                        {
                            Msg = entry.Message,
                            Type = toStringFromMessageTypeEnum(entry.Status)
                        };
                        Logs.Insert(0, logWeb);
                    }
                    ifLogUpdate = true;

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// convert from msg to string
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string toStringFromMessageTypeEnum(MessageTypeEnum type)
        {
            if (type == MessageTypeEnum.WARNING)
                return "WARNING";
            else if (type == MessageTypeEnum.FAIL)
                return "FAIL";
            else
                return "INFO";

        }
        
        /// <summary>
        /// properties
        /// </summary>
        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "logList:   ")]
        public List<LogWeb> Logs { get; set; }

    }
}


