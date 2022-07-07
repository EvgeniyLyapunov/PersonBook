using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLibrary.Models
{
    public class ApplicationUser
    {
        [Key]
        public int Id { get; set; } 
        [EmailAddress]
        public string Name { get; set; }
        public string Password { get; set; }   
        

    }
}
