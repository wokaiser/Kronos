﻿using KronosData.Model;
using System;
using System.Globalization;
using System.Windows.Data;

namespace KronosUI.Converters
{
    public class DateUnitToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is WorkDate))
            {
                throw new ArgumentException(string.Format("{0} is not from type {1}", nameof(value), nameof(WorkDate)));
            }

            var tmp = (value as WorkDate);

            switch (parameter.ToString().ToUpper())
            {
                default:
                case "DATE":
                    return tmp.DateOfWork.ToShortDateString();

                case "BEGIN":
                    return tmp.Begin.Hours == tmp.End.Hours && tmp.Begin.Minutes == tmp.End.Minutes ? string.Empty : tmp.Begin.ToString(@"hh\:mm");

                case "END":
                    return tmp.Begin.Hours == tmp.End.Hours && tmp.Begin.Minutes == tmp.End.Minutes ? string.Empty : tmp.End.ToString(@"hh\:mm");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
