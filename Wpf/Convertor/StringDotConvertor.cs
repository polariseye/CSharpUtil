namespace Polaris.Utility.Wpf
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class StringDotConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "";
            }

            try
            {
                var originString = value.ToString();

                var maxLength = System.Convert.ToInt32(parameter);
                if (maxLength > originString.Length)
                {
                    return originString;
                }

                return originString.Substring(0, maxLength) + "...";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"convert error:{ex}");
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
