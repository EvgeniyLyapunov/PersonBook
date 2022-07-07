using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLibrary.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Last Name")]
        [Required]
        public string LastName { get; set; }
        [DisplayName("First Name")]
        [Required]
        public string FirstName { get; set; }
        [DisplayName("Phone Number")]
        [Required]
        public string PhoneNumber { get; set; }
        public string EMail { get; set; }
        [DisplayName("Additional Information")]
        public string AdditionalInfo { get; set; }
        public string UserId { get; set; }
        public string PictureName { get; set; }
        public byte[] Avatar { get; set; }

    }
}
