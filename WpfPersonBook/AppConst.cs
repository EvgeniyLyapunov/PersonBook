using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfPersonBook
{
    internal class AppConst
    {
        public const string ApiPath = @"http://localhost:63810";

        //public const string ApiPath = @"http://apiservicepersonbook.somee.com";

        public static int MaxSizeOfPicture = 102400;

        public static byte[] GetDefaultAvatar()
        {
            byte[] imageData = null;
            var p = System.AppDomain.CurrentDomain.BaseDirectory;
            string picPath = $@"{p}\noavatar.png";
            imageData = File.ReadAllBytes(picPath);
            return imageData;
        }
    }
}
