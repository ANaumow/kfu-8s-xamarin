using System;
using System.Globalization;
using Xamarin.Forms;

namespace App5
{
    public class StringToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = (string)value;
            return String.IsNullOrWhiteSpace(s) ? 0d : Double.Parse(s);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((double) value) + "";
        }
    }
}