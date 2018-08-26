using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    class ConfigParse
    {
        public int id;
        public string sourceName; //{ get; set; }
        public string outputDir; //{ get; set; }
        public int thumbnailSize; //{ get; set; }
        public string logName; //{ get; set; }
        public string hendlers;// { get; set; }

        public ConfigParse()
        {
            this.id = (int)CommandEnum.GetConfigCommand;
            this.sourceName = ConfigurationManager.AppSettings["SourceName"];
            this.logName =ConfigurationManager.AppSettings["LogName"];//ImageServiceLog
            this.thumbnailSize = int.Parse(ConfigurationManager.AppSettings["ThumbnailSize"]);
            this.outputDir = ConfigurationManager.AppSettings["OutputDir"];
            this.hendlers= ConfigurationManager.AppSettings["Hendlers"];
        }


    }
}
