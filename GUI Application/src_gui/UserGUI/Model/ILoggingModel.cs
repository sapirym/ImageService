using ImageService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserGUI.Model
{
    interface ILoggingModel : INotifyPropertyChanged
    {
        ObservableCollection<LogMessege> Logs { get; set; }
        void getHistory();
        void messageRecieved(object sender, DataInfo info);
    }
}
