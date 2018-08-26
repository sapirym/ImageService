using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace UserGUI
{
    class boolToConvert : IValueConverter
    {
        /// <summary>
        /// the function get value and according is type- change the color.
        /// in this case- if the value=true=> white. else =>grey
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool connect = (bool)value;
            if (connect)
                return Color.FromRgb(255,255,255); //succed to connect - White
            return Color.FromRgb(114, 102, 102); ////fail to connect - Grey
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
