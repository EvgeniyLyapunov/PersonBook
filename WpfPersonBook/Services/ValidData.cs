using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WpfPersonBook.Services
{
    public static class ValidData
    {
        public static bool IsEmailValid(string email)
        {
            Regex r = new Regex(@"^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\s*[;,.]{0,1}\s*)+$");
            return r.IsMatch(email);
        }

        public static bool IsPhoneValid(string phone)
        {
            Regex r = new Regex(@"^[1-9]+[0-9]*$");
            return r.IsMatch(phone);
        }
    }
}
