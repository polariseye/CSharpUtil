namespace Polaris.Utility.Wpf
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Controls;
    using System.Windows.Interop;

    public class EmbeddedApp : HwndHost, IKeyboardInputSink
    {
        public Border WndHoster { get; private set; }
        private System.Diagnostics.Process appProc;
        private uint oldStyle;
        public IntPtr hwndHost;
        public EmbeddedApp(Border b, Int32 hwnd)
        {
            WndHoster = b;
            this.hwndHost = new IntPtr(hwnd);
        }

        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            //User32Api.SendMessage(hwndHost, User32Api.WM_SYSCOMMAND, User32Api.SC_MINIMIZE, 0);

            // 嵌入在HwnHost中的窗口必须要 设置为WS_CHILD风格    
            oldStyle = User32Api.GetWindowLong(hwndHost, User32Api.GWL_STYLE);
            uint newStyle = oldStyle;
            //WS_CHILD和WS_POPUP不能同时存在。有些Win32窗口，比如QQ的窗口，有WS_POPUP属性，这样嵌入的时候会导致程序错误   
            newStyle |= User32Api.WS_CHILD;
            newStyle &= ~User32Api.WS_POPUP;
            newStyle &= ~User32Api.WS_BORDER;
            User32Api.SetWindowLong(hwndHost, User32Api.GWL_STYLE, newStyle);

            //将netterm的父窗口设置为HwndHost    
            User32Api.SetParent(hwndHost, hwndParent.Handle);
            //MoveWindow(hwndHost, 0, 0, 100, 100, true);
            //ShowWindow(this.hwndHost, SW_SHOWNA);
            return new HandleRef(this, hwndHost);
        }
        protected override void DestroyWindowCore(System.Runtime.InteropServices.HandleRef hwnd)
        {
            //User32Api.SetWindowLong(hwndHost, User32Api.GWL_STYLE, oldStyle);
            User32Api.SetParent(hwndHost, (IntPtr)0);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (hwndHost.ToInt32() != 0)
            {
                User32Api.SetWindowLong(hwndHost, User32Api.GWL_STYLE, oldStyle);
                User32Api.SetParent(hwndHost, (IntPtr)0);
            }
        }

        public void Close()
        {
            User32Api.SetWindowLong(hwndHost, User32Api.GWL_STYLE, oldStyle);
            User32Api.SetParent(hwndHost, (IntPtr)0);
            this.hwndHost = new IntPtr(0);
        }
    }
}