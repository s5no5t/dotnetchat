using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DotNetChat.Converter
{
    public class InverseConnectedToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var connected = (bool)value;
            return connected ? Visibility.Hidden : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}