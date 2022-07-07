using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfPersonBook.Models
{
    public class WpfContact
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PhoneNumber { get; set; }
        public string EMail { get; set; }
        public string AdditionalInfo { get; set; }
        public string UserId { get; set; }
        public string PictureName { get; set; }
        public BitmapImage Avatar { get; set; }

    }
}
