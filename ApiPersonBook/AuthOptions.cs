using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiPersonBook
{
    public class AuthOptions
    {
        public const string ISSUER = "http://localhost:63810"; // издатель токена
        public const string AUDIENCE = "http://localhost:63810"; // потребитель токена
        const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
        public const int LIFETIME = 30; // время жизни токена - 30 минут
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
