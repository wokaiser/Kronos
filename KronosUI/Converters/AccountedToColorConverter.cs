using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace KronosUI.Converters
{
    public class AccountedToColorConverter : MarkupExtension, IMultiValueConverter
    {
        public AccountedToColorConverter() 
        {
            HighlightColor = new SolidColorBrush(Colors.DarkRed);
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
            {
                return new SolidColorBrush(Colors.Black);
            }

            if (string.IsNullOrWhiteSpace(values[0].ToString()) || string.IsNullOrWhiteSpace(values[1].ToString()))
            {
                return new SolidColorBrush(Colors.Black);
            }            

            return values[0].ToString() == values[1].ToString() ? new SolidColorBrush(Colors.Black) : HighlightColor;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public Brush HighlightColor { get; set; }
    }
}
