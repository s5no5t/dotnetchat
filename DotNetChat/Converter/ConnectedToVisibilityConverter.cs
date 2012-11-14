using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DotNetChat.Converter
{
    public class ConnectedToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var connected = (bool) value;
            return connected ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}