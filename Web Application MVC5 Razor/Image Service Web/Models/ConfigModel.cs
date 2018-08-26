using ImageService;
using ImageServiceWeb.Communication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class ConfigModel
    {
        private bool ifConfUpdate;

        /// <summary>
        /// constructor of config model
        /// </summary>
        public ConfigModel()
        {
                clientTcpConnectionWeb singelton = clientTcpConnectionWeb.Instance;
                string serviceConnect = singelton.IsServiceConnect();
            ifConfUpdate = false;
                if (serviceConnect.Equals("Connect"))
                {
                Handlers = new ObservableCollection<string>();
                singelton.MessageReceived += getConf;
                    DataInfo msg = new DataInfo(CommandEnum.GetConfigCommand, null);
                    singelton.WriteToServer(msg.toJson());
                    while (!ifConfUpdate)
                    {
                        Thread.Sleep(100);
                    }
                }
            }

        /// <summary>
        /// ger config function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="msg"></param>
        private void getConf(object sender, DataInfo msg)
        {
            if (msg.Id == CommandEnum.GetConfigCommand)
            {
                string[] arr = JsonConvert.DeserializeObject<string[]>(msg.Args);
                OutputDir = arr[1];
                SourceName = arr[2];
                LogName = arr[3];
                ThumbnailSize = (string)arr[4];
                string hanslersToSplit = arr[0];
                string[] temp = hanslersToSplit.Split(';');
                Handlers.Clear();
                foreach (string item in temp)
                    Handlers.Add(item);
                ifConfUpdate = true;

            }
        }

        /// <summary>
        /// required : handlers
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Handlers:  ")]
        public ObservableCollection<string> Handlers { get; set; }

        /// <summary>
        /// required :outputdir
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "OutputDir: ")]
        public string OutputDir { get; set; }

        /// <summary>
        /// required : sourename
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "SourceName:    ")]
        public string SourceName { get; set; }

        /// <summary>
        /// required : logName
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "LogName:   ")]
        public string LogName { get; set; }

        /// <summary>
        /// required : thumbsize
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ThumbnailSize: ")]
        public string ThumbnailSize { get; set; }

    
    }
}