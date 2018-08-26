using ImageService;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace UserGUI.ViewModel
{

    class ConvertColor : IValueConverter
    {
        /// <summary>
        /// the function get value and according is type- change the color.
        /// in this case- if the value=FAIL=> red. else if value=WARNING =>yellow, else=>green
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
                throw new InvalidOperationException("Must convert to a brush!");

            MessageTypeEnum Type = (MessageTypeEnum)value;

            return (Type == MessageTypeEnum.FAIL) ? Brushes.Red :
                (Type == MessageTypeEnum.WARNING) ? Brushes.Yellow :
                (Type == MessageTypeEnum.INFO) ? Brushes.Green :
                 Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }



}