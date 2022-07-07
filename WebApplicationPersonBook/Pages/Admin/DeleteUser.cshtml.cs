using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApplicationPersonBook.AuthData;
using WebApplicationPersonBook.Services;

namespace WebApplicationPersonBook.Pages.Admin
{
    public class DeleteUserModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly HttpClient _httpClient;

        private IApiAuth _apiAuth { get; }
        public DeleteUserModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, HttpClient httpClient, IApiAuth apiAuth)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _httpClient = httpClient;
            _apiAuth = apiAuth;
        }

        [BindProperty(SupportsGet = true)]
        public InputModel Input { get; set; }


        public class InputModel
        {
            public string UserName { get; set; }
            public string UserRole { get; set; }

            public IdentityUser deleteUser { get; set; }
        }


        public void OnGet(string UserName)
        {
            Input.deleteUser = _userManager.Users.SingleOrDefault(u => u.UserName == UserName);

            Input.UserName = UserName;
            Input.UserRole = _userManager.GetRolesAsync(Input.deleteUser).GetAwaiter().GetResult()[0];
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.FindByEmailAsync(Input.UserName);
            var apiUserName = Input.UserName;

            var token = HttpContext.Session.GetString("Token");

            var apiAnswer = await _apiAuth.ApiUserDelete("deleteuser", apiUserName, token);
            if (apiAnswer.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ViewData["Message"] = apiAnswer.ReasonPhrase;
                return Page();
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return RedirectToPage("/Admin/UsersList");
            }

            return Page();
        }

    }
}
