using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace ImageService
{
    public class CommandRecievedEventArgs : EventArgs
    {
        public string args;

        public int CommandID { get; set; }      // The Command ID
        public string[] Args { get; set; }
        public string RequestDirPath { get; set; }  // The Request Directory

        public CommandRecievedEventArgs(int id, string[] args, string path)
        {
            CommandID = id;
            Args = args;
            RequestDirPath = path;
        }

        public string toJson()
        {
            JObject cmdObj = new JObject();
            cmdObj["CommandID"] = CommandID;

            JArray args = new JArray(Args);
            cmdObj["CommandArgs"] = args;
            return cmdObj.ToString();
        }


        public static CommandRecievedEventArgs fromJson(string str)
        {
            JObject cmdObj = JObject.Parse(str);
            int cmdId = (int)cmdObj["CommandID"];
            JArray arr = (JArray)cmdObj["CommandArgs"];
            CommandRecievedEventArgs cmd = new CommandRecievedEventArgs(cmdId, arr.Select(c => (string)c).ToArray(), null);
            return cmd;
        }
    }
}
