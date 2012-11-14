using System;
using System.Globalization;
using System.Windows.Data;

namespace DotNetChat.Converter
{
    public class ConnectedStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Connected" : "Not Connected";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}