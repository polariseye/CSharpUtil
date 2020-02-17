namespace Polaris.Utility.Wpf
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    public class EmbedHelper : DependencyObject
    {
        public static Int32 GetMainWindowHandle(DependencyObject obj)
        {
            return (Int32)obj.GetValue(MainWindowHandleProperty);
        }

        public static void SetMainWindowHandle(DependencyObject obj, Int32 value)
        {
            obj.SetValue(MainWindowHandleProperty, value);
        }

        public static readonly DependencyProperty MainWindowHandleProperty =
            DependencyProperty.RegisterAttached("MainWindowHandle", typeof(Int32), typeof(EmbedHelper), new PropertyMetadata(0, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var controlObj = d as Border;
            var oldVal = (Int32)e.OldValue;
            var nowVal = (Int32)e.NewValue;

            if (oldVal != 0)
            {
                // 解除关联
                EmbeddedApp oldEmbed = controlObj.Tag as EmbeddedApp;
                controlObj.Child = null;
                if (oldEmbed != null)
                {
                    oldEmbed.Close();
                }
            }

            if (nowVal == 0)
            {
                return;
            }

            EmbeddedApp a = new EmbeddedApp(controlObj, nowVal);
            controlObj.Tag = a;
            controlObj.Child = a;
        }

        public static Boolean CheckWindowExist2(Int64 hwnd)
        {
            User32Api.RECT ret = new User32Api.RECT();
            var result = User32Api.GetWindowRect(new IntPtr(hwnd), ref ret);
            if (result == 1)
            {
                return true;
            }

            return false;
        }
    }
}
