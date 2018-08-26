using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    //id in dictionary for command
    public enum CommandEnum : int
    {
        NewFileCommand = 1,
        GetConfigCommand = 2,
        LogCommand = 3,
        CloseCommand = 4,
        CloseAll = 5,
        newLogEntryCommand = 6
    }
}
