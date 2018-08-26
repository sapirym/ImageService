using ImageService;
using Microsoft.Practices.Prism.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserGUI.Communication;
using UserGUI.Model;

namespace UserGUI.ViewModel
{
    class MainWindowVM : INotifyPropertyChanged
    {
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
        #region properties
        private MainWindowModel model;
        public bool Connect
        {
            get { return model.Connect; }
        }
        #endregion
        /// <summary>
        /// constructor of MainWindowVM
        /// </summary>
        /// <param name="model"></param>
        public MainWindowVM(MainWindowModel model)
        {
            this.model = model;
            model.PropertyChanged +=
            delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged(e.PropertyName);
            };

        }  
    }
}

