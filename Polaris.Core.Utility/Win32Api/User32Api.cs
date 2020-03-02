namespace Polaris.Utility
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class User32Api
    {
        [DllImport("user32.dll")]
        public static extern int SetParent(IntPtr hWndChild, IntPtr hWndParent);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint SetWindowLong(IntPtr hwnd, int nIndex, uint newLong);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint GetWindowLong(IntPtr hwnd, int nIndex);
        [DllImport("user32.dll")]
        public static extern int EnumWindows(CallBackPtr callPtr, ref WindowInfo WndInfoRef);
        [DllImport("User32.dll")]
        public static extern int GetWindowText(IntPtr handle, StringBuilder text, int MaxLen);
        [DllImport("user32.dll")]
        public static extern int GetWindowRect(IntPtr hwnd, ref RECT rc);
        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hwnd, uint wMsg, int wParam, int lParam); //对外部软件窗口发送一些消息(如 窗口最大化、最小化等)

        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

        [DllImport("user32.dll", EntryPoint = "MoveWindow", SetLastError = true)]
        public static extern int MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool BRePaint);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        public static extern int FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, String lpszClass, String lpszWindow);

        [DllImport("user32.dll", EntryPoint = "SwitchToThisWindow", SetLastError = true)]
        public static extern int SwitchToThisWindow(IntPtr hwnd, Boolean isActive);

        /// <summary>
        /// 保存文件信息的结构体
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        public const uint SHGFI_ICON = 0x100;
        public const uint SHGFI_LARGEICON = 0x0; //大图标 32×32
        public const uint SHGFI_SMALLICON = 0x1; //小图标 16×16
        public const uint SHGFI_USEFILEATTRIBUTES = 0x10;
        [DllImport("Shell32.dll", EntryPoint = "SHGetFileInfo", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

        [DllImport("User32.dll", EntryPoint = "DestroyIcon")]
        public static extern int DestroyIcon(IntPtr hIcon);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public int Width
            {
                get
                {
                    return (this.Right - this.Left);
                }
            }
            public int Height
            {
                get
                {
                    return (this.Bottom - this.Top);
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WindowInfo
        {
            public String winTitle;
            public RECT r;
            public IntPtr hwnd;
        }

        public delegate bool CallBackPtr(IntPtr hwnd, ref WindowInfo WndInfoRef);

        public const int
           GWL_WNDPROC = (-4),
           GWL_HINSTANCE = (-6),
           GWL_HWNDPARENT = (-8),
           GWL_STYLE = (-16),
           GWL_EXSTYLE = (-20),
           GWL_USERDATA = (-21),
           GWL_ID = (-12);
        public const uint
              WS_CHILD = 0x40000000,
              WS_VISIBLE = 0x10000000,
              LBS_NOTIFY = 0x00000001,
              HOST_ID = 0x00000002,
              LISTBOX_ID = 0x00000001,
              WS_VSCROLL = 0x00200000,
              WS_BORDER = 0x00800000,
              WS_POPUP = 0x80000000;
        public const int HWND_TOP = 0x0;
        public const int WM_COMMAND = 0x0112;
        public const int WM_QT_PAINT = 0xC2DC;
        public const int WM_PAINT = 0x000F;
        public const int WM_SIZE = 0x0005;
        public const int SWP_FRAMECHANGED = 0x0020;
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_CLOSE = 0xF060;
        public const int SC_MINIMIZE = 0xF020;
        public const int SC_MAXIMIZE = 0xF030;
        public const uint WM_LBUTTONDOWN = 0x0201;
        public const uint WM_LBUTTONUP = 0x0202;
        public const uint WS_EX_NOACTIVATE = 0x08000000;
        /// <summary>
        /// 关闭窗口的消息
        /// </summary>
        public const int WM_CLOSE = 0x10;
        public const int BM_CLICK = 0xF5;

        public const int SW_HIDE = 0;
        public const int SW_MAXIMIZE = 3;
        public const int SW_SHOW = 5;
        public const int SW_SHOWNA = 8;
    }
}
