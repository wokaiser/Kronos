using System;
using System.Globalization;
using System.Windows.Data;

namespace KronosUI.Converters
{
    public class TimeSpanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is TimeSpan))
            {
                throw new ArgumentException(string.Format("{0} is not from type {1}", nameof(value), nameof(TimeSpan)));
            }

            if (!System.Convert.ToBoolean(parameter) && ((TimeSpan)value).Ticks == 0)
            {
                return string.Empty;
            }

            return ((TimeSpan)value).ToString(@"hh\:mm");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
