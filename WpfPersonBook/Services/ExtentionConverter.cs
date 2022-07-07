using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfPersonBook.Services
{
    public static class ExtentionConverter
    {
        public static BitmapImage ConvertToBitmapimage(this byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            stream.Seek(0, SeekOrigin.Begin);
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }

        public static byte[] ConvertToByteArray(this BitmapImage image)
        {
            byte[] bytes;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(source: image));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                bytes = ms.ToArray();
            }
            return bytes;
        }
    }
}
