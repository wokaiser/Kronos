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
            if (value is TimeSpan)
            {
                return new SolidColorBrush((TimeSpan)value > TimeSpan.Zero ? Colors.Black : Colors.Red);
            }

            if (value is string)
            {
                var val = value as string;

                if (string.IsNullOrEmpty(val) || val[0] != '-')
                {
                    return new SolidColorBrush(Colors.Black);
                }

                return new SolidColorBrush(Colors.DarkRed);
            }

            return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
