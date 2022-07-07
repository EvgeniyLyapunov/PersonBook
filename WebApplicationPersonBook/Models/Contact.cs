using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationPersonBook.Models
{
    public class Contact
    {
            [Key]
            public int Id { get; set; }
            [DisplayName("Last Name")]
            [StringLength(20)]
            [Required]
            public string LastName { get; set; }
            [DisplayName("First Name")]
            [StringLength(20)]
            [Required]
            public string FirstName { get; set; }
            [DisplayName("Phone Number")]
            [Phone]
            [Required]
            public string PhoneNumber { get; set; }
            [EmailAddress]
            public string EMail { get; set; }
            [DisplayName("Additional Information")]
            public string AdditionalInfo { get; set; }
            public string UserId { get; set; }
            public string PictureName { get; set; }
            public byte[] Avatar { get; set; }


    }
}
