
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    class LogCommand : ICommand
    {
        //  private IImageServiceModal modal;
        private ILoggingService log;

        public LogCommand(ILoggingService log)
        {
            this.log = log;
        }

        public string Execute(string[] args, out bool result)
        {
            
            try
            {
                ConfigParse appConfig = new ConfigParse();
                EventLog log = new EventLog("MyLogFile1");
                EventLogEntryCollection entries = log.Entries;
                List<MessageRecievedEventArgs> logEntries = new List<MessageRecievedEventArgs>();
                foreach (EventLogEntry entry in entries)
                {
                    Console.Write("add entry log");
                    logEntries.Add(new MessageRecievedEventArgs(
                        MessageRecievedEventArgs.toMessageTypeEnum(entry.EntryType), entry.Message));
                }
                List<MessageRecievedEventArgs> argsLog = logEntries;
                string argsJson = JsonConvert.SerializeObject(argsLog);
                DataInfo inf = new DataInfo(CommandEnum.LogCommand, argsJson);
                string infJson = JsonConvert.SerializeObject(inf);
                result = true;
                return infJson;
            }
            catch (Exception e)
            {
                result = false;
                Debug.WriteLine(e.Message);
                return null;
            }
        }
    }
}

/*
 * AppConfigParser appConfig = new AppConfigParser();
                EventLog log = new EventLog(appConfig.logName, ".");
                EventLogEntryCollection entries = log.Entries;
                
                List<Entry> logEntries = new List<Entry>();
                foreach (EventLogEntry entry in entries)
                {
                    logEntries.Add(new Entry(entry.Message, Entry.toMessageTypeEnum(entry.EntryType)));
                }

                string convertEachString;
                if ((convertEachString = JsonConvert.SerializeObject(logEntries)) == null)
                {
                    result = false;
                    return null;
                }

                result = true;

                string[] arrForMsg = new string[1];
                arrForMsg[0] = convertEachString;
                MessageCommand msg = new MessageCommand((int)CommandEnum.LogCommand, arrForMsg, null);
                return JsonConvert.SerializeObject(msg);
            }
            catch (Exception e)
            {
                result = false;
                Debug.WriteLine(e.Message);
                return null;
            }
 * */
