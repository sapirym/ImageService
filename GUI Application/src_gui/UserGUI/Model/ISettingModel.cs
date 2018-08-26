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
    public delegate void SendToService(DataInfo msg);

    interface ISettingModel : INotifyPropertyChanged
    {
        string SourceName { get; set; }
        string LogName { get; set; }
        string OutputDir { get; set; }
        string ThumbnailSize { get; set; }
        ObservableCollection<string> Handler { get; set; }
        string ChooseHendler { get; set; }

        void getFirstConfi();
        void onGetConfig(object sender, DataInfo msg);
        //void CloseHendler(string handler);

    }
}



