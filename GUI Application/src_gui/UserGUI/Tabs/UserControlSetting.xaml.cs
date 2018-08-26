using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserGUI.View;

namespace UserGUI.Tabs
{
    /// <summary>
    /// Interaction logic for UserControlSetting.xaml
    /// </summary>
    public partial class UserControlSetting : UserControl
    {
        private SettingViewModel vm;

        /// <summary>
        /// constructor of UserControlSetting 
        /// </summary>
        public UserControlSetting()
        {
            InitializeComponent();
            this.DataContext = new SettingViewModel();
        }

    }
}
