

namespace Polaris.Utility.WinForm
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Media;

    public class PixelUtil
    {
        public static System.Windows.Size GetStringPxSize(String val, System.Windows.Media.FontFamily fontFamily, System.Windows.FontStyle fontStyle, FontWeight weight, FontStretch stretch, double fontSize)
        {
            if (String.IsNullOrWhiteSpace(val))
            {
                return new System.Windows.Size();
            }

            var formattedText = new FormattedText(
                val,
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface(fontFamily, fontStyle, weight, stretch),
                fontSize,
                System.Windows.Media.Brushes.Black);

            System.Windows.Size size = new System.Windows.Size(formattedText.Width, formattedText.Height);
            return size;
        }
    }
}
