﻿namespace Polaris.Utility.Wpf
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public class Object2VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
