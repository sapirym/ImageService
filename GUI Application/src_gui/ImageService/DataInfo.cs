using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    public class DataInfo
    {
        public delegate void UpdateUsers(DataInfo msg);

        private CommandEnum id;
        private string args;
        public CommandEnum Id {
            get {
                return this.id;
            } set {
                this.id = value;
                    } }
        public string Args {
            get {
                return this.args;
            }
            set
            {
                this.args = value;
            }
        }

        public DataInfo(CommandEnum Id,  string args)
        {
            this.Id = Id;
            this.args = args;
        }
        public string toJson()
        {
            return JsonConvert.SerializeObject(this);
            
        }


        public static DataInfo fromJson(string str)
        {
           // JObject cmdObj = JObject.Parse(str);
            //int cmdId = (int)cmdObj["CommandID"];
            //JObject arr = (JObject)cmdObj["CommandArgs"];
            return JsonConvert.DeserializeObject<DataInfo>(str);

            //MsgInfo cmd = new MsgInfo((CommandEnum)cmdId, msg);//.Select(c => (string)c).ToArray(), null);
            //return cmd;
        }
    }
}
