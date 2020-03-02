
namespace WechatBussiness.Common
{
    using System.Drawing;
    using System.IO;
    using System.Windows;
    using System.Windows.Interop;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public class ImageUtil
    {
        public static ImageSource GetIcon(string FilePath)
        {
            FilePath = FilePath.Replace("/", @"\");
            ImageSource source = null;
            if ((source != null) || !File.Exists(FilePath))
            {
                return source;
            }
            using (Icon icon = Icon.ExtractAssociatedIcon(FilePath))
            {
                return Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
        }

        public static BitmapImage GetImage(string imagepath)
        {
            FileStream stream1 = new FileStream(imagepath, FileMode.Open);
            byte[] buffer = new byte[stream1.Length];
            stream1.Read(buffer, 0, buffer.Length);
            stream1.Close();
            BitmapImage image = null;
            try
            {
                image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = new MemoryStream(buffer);
                image.EndInit();
            }
            catch
            {
                image = null;
            }
            return image;
        }
    }
}
