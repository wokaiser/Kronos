using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace KronosUI.Converters
{
    public class ValueToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = value as string;

            if (val == null || val[0] != '-')
            {
                return new SolidColorBrush(Colors.Black);
            }

            return new SolidColorBrush(Colors.DarkRed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
