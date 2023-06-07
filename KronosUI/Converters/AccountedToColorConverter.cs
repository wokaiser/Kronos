using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace KronosUI.Converters
{
    public class AccountedToColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
            {
                return new SolidColorBrush(Colors.Black);
            }

            if (string.IsNullOrWhiteSpace(values[0] as string) || string.IsNullOrWhiteSpace(values[1] as string))
            {
                return new SolidColorBrush(Colors.Black);
            }

            return new SolidColorBrush(values[0] as string == values[1] as string ? Colors.Black : Colors.DarkRed);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
