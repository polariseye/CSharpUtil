namespace Polaris.Utility
{
    using System;
    using System.Runtime.InteropServices;

    public static class Kernel32Api
    {
        //加载DLL
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public extern static IntPtr LoadLibrary(string path);

        //获取函数地址
        [DllImport("kernel32.dll", SetLastError = true)]
        public extern static IntPtr GetProcAddress(IntPtr lib, string funcName);

        //释放相应的库
        [DllImport("kernel32.dll")]
        public extern static bool FreeLibrary(IntPtr lib);

        //获取错误信息
        [DllImport("Kernel32.dll")]
        public extern static int FormatMessage(int flag, ref IntPtr source, int msgid, int langid, ref string buf, int size, ref IntPtr args);
    }
}
