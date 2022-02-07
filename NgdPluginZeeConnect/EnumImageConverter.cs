using Ngd.Dialog;
using System;
using System.Globalization;
using System.Windows.Data;

namespace NgdPluginZeeConnect
{
    public class EnumImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return $@"pack://application:,,,/NgdPluginZeeConnect;component/Resources/images/{value.ImageName()}";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
