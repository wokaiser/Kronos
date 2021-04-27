using KronosData.Model;
using System;
using System.Globalization;
using System.Windows.Data;

namespace KronosUI.Converters
{
    public class DateUnitToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DateUnit))
            {
                throw new ArgumentException(string.Format("{0} is not from type {1}", nameof(value), nameof(DateUnit)));
            }

            var tmp = (value as DateUnit);

            switch (parameter.ToString().ToUpper())
            {
                default:
                case "DATE":
                    return tmp.Begin.ToShortDateString();

                case "BEGIN":
                    return tmp.Begin.Hour == tmp.End.Hour && tmp.Begin.Minute == tmp.End.Minute ? string.Empty : tmp.Begin.ToShortTimeString();

                case "END":
                    return tmp.Begin.Hour == tmp.End.Hour && tmp.Begin.Minute == tmp.End.Minute ? string.Empty : tmp.End.ToShortTimeString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
