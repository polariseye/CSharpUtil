namespace Polaris.Utility.Wpf
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Interop;

    public static class WindowHelper
    {
        /// <summary>
        ///     获取当前应用中处于激活的一个窗口
        /// </summary>
        /// <returns></returns>
        public static Window GetActiveWindow() => Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);

        public static IntPtr CreateHandle() => new WindowInteropHelper(new Window()).EnsureHandle();

        public static IntPtr GetHandle(this Window window) => new WindowInteropHelper(window).EnsureHandle();
    }
}
