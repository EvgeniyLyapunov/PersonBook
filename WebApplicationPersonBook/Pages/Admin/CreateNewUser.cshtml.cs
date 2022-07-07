using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebApplicationPersonBook.AuthData;
using WebApplicationPersonBook.Services;

namespace WebApplicationPersonBook.Pages.Admin
{
    public class CreateNewUserModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IApiAuth _apiAuth { get; }

        public CreateNewUserModel(SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IApiAuth apiAuth)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _apiAuth = apiAuth;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Required]
            [Display(Name = "User role")]
            public string UserRole { get; set; }
        }


        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var unicEmail = _userManager.FindByEmailAsync(Input.Email).Result;
            if (unicEmail != null)
            {
                ViewData["Message"] = "ѕользователь с таким Email уже существует.";
                return Page();
            }

            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var apiUser = new ApiUser() { Name = Input.Email, Password = Input.Password };

                var token = HttpContext.Session.GetString("Token"); 

                var apiAnswer = await _apiAuth.ApiUserCreation("createuser", apiUser, token);
                if (apiAnswer.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ViewData["Message"] = apiAnswer.ReasonPhrase;
                    return Page();
                }

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Input.UserRole);
                }

                return RedirectToPage("/Admin/UsersList");
            }

            return Page();
        }
    }
}
