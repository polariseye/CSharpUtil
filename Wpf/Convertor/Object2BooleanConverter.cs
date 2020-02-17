namespace Polaris.Utility.Wpf
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class Object2BooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => !(value is null);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}