using System.IO;

namespace WebApplicationPersonBook
{
    public static class AppConst
    {
        public const int FirstRegistration = 1;

        public const string AdminRole = "Admin";
        public const string UserRole = "User";

        public static int MaxSizeOfPicture = 102400;

        public const string DefaultAvatarName = "DefaultAvatar";

        public static byte[] GetDefaultAvatar(string rootPath)
        {
            byte[] imageData = null;
            string picPath = rootPath + @"/defaultAvatar/noavatar.png";
            imageData = File.ReadAllBytes(picPath);
            return imageData;
        }

        public const string ApiPath = @"http://localhost:63810";
    }
}
