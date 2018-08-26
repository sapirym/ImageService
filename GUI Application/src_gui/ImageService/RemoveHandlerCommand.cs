using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    class RemoveHandlerCommand : ICommand
    {
        /// <summary>
        /// execute the remove command
        /// </summary>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public string Execute(string[] args, out bool result)
        {
            string[] array = ConfigurationManager.AppSettings["Handler"].Split(';');
            foreach (string s in array)
            {
                if (s.Equals(args[0]))
                {
                    array = array.Where(val => val != args[0]).ToArray();
                    string joined = String.Join(";", array);
                    ConfigurationManager.AppSettings["Handler"] = joined;
                    result = true;
                    DataInfo data = new DataInfo(CommandEnum.CloseCommand, args[0]);
                    return data.toJson();
                }
            }
            result = false;
            return "the handler doesn't exist. Can't remove it.";
        }
    }
}
