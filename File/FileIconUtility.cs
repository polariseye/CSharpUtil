namespace Polaris.Utility
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    /// <summary>
    /// 获取指定文件的Icon图像getIcon()、getIcon2()
    /// </summary>
    public class FileIconUtility
    {
        /// <summary>
        /// 获取文件类型的关联图标
        /// </summary>
        /// <param name="fileName">文件类型的扩展名或文件的绝对路径</param>
        /// <param name="isLargeIcon">是否返回大图标</param>
        /// <returns>获取到的图标</returns>
        static Icon GetIcon(string fileName, bool isLargeIcon)
        {
            User32Api.SHFILEINFO shfi = new User32Api.SHFILEINFO();
            IntPtr hI;

            if (isLargeIcon)
                hI = User32Api.SHGetFileInfo(fileName, 0, ref shfi, (uint)Marshal.SizeOf(shfi), User32Api.SHGFI_ICON | User32Api.SHGFI_USEFILEATTRIBUTES | User32Api.SHGFI_LARGEICON);
            else
                hI = User32Api.SHGetFileInfo(fileName, 0, ref shfi, (uint)Marshal.SizeOf(shfi), User32Api.SHGFI_ICON | User32Api.SHGFI_USEFILEATTRIBUTES | User32Api.SHGFI_SMALLICON);

            Icon icon = Icon.FromHandle(shfi.hIcon).Clone() as Icon;

            User32Api.DestroyIcon(shfi.hIcon); //释放资源
            return icon;
        }
        /// <summary>      
        /// 获取文件图标    
        /// </summary>      
        /// <param name="p_Path">文件全路径</param>      
        /// <returns>图标</returns>     
        public static Icon GetFileIcon(string p_Path)
        {
            User32Api.SHFILEINFO fileIconInfo = new User32Api.SHFILEINFO();
            IntPtr _IconIntPtr = User32Api.SHGetFileInfo(p_Path, 0, ref fileIconInfo, (uint)Marshal.SizeOf(fileIconInfo), (uint)(User32Api.SHGFI_ICON | User32Api.SHGFI_LARGEICON | User32Api.SHGFI_USEFILEATTRIBUTES));
            if (_IconIntPtr.Equals(IntPtr.Zero))
                return null;

            if (fileIconInfo.iIcon == 0)
            {
                return null;
            }

            Icon _Icon = System.Drawing.Icon.FromHandle(fileIconInfo.hIcon);
            return _Icon;
        }

        /// <summary>
        /// 获取文件夹图标
        /// </summary>
        /// <returns></returns>
        public static Icon GetDirectoryIcon()
        {
            User32Api.SHFILEINFO _SHFILEINFO = new User32Api.SHFILEINFO();
            IntPtr _IconIntPtr = User32Api.SHGetFileInfo(@"", 0, ref _SHFILEINFO, (uint)Marshal.SizeOf(_SHFILEINFO), (uint)(User32Api.SHGFI_ICON | User32Api.SHGFI_LARGEICON));
            if (_IconIntPtr.Equals(IntPtr.Zero))
                return null;
            Icon _Icon = System.Drawing.Icon.FromHandle(_SHFILEINFO.hIcon);
            return _Icon;
        }
    }
}
