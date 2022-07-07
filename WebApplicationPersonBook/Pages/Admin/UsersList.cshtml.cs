using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace WebApplicationPersonBook.Pages.Admin
{
    public class UsersListModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UsersListModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            Users = new List<InputModel>();
        }

        public InputModel Input { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<InputModel> Users { get; set; }

        public class InputModel
        {
            public string UserName { get; set; }
            public string UserRole { get; set; }
        }

        
        
        public void OnGet()
        {
            var users = _userManager.Users.ToList();

            foreach (var user in users)
            {
                Users.Add(new InputModel()
                {
                    UserName = user.UserName,
                    UserRole = _userManager.GetRolesAsync(user).GetAwaiter().GetResult()[0]
                });
            }

        }
    }
}
