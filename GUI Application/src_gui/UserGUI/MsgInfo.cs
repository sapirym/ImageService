using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    class MsgInfo
    {
        public delegate void UpdateResponseArrived(MsgInfo msg);

        public CommandEnum Id { get; set; }
        public string args { get; set; }

        public MsgInfo(CommandEnum Id,  string args)
        {
            this.Id = Id;
            this.args = args;
        }
        public string toJson()
        {
            return JsonConvert.SerializeObject(this);
            
        }


        public static MsgInfo fromJson(string str)
        {
           // JObject cmdObj = JObject.Parse(str);
            //int cmdId = (int)cmdObj["CommandID"];
            //JObject arr = (JObject)cmdObj["CommandArgs"];
            return JsonConvert.DeserializeObject<MsgInfo>(str);

            //MsgInfo cmd = new MsgInfo((CommandEnum)cmdId, msg);//.Select(c => (string)c).ToArray(), null);
            //return cmd;
        }
    }
}
