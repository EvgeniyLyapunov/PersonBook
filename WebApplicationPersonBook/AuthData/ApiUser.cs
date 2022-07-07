using System.ComponentModel.DataAnnotations;

namespace WebApplicationPersonBook.AuthData

{
    public class ApiUser
    {
        [EmailAddress]
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
