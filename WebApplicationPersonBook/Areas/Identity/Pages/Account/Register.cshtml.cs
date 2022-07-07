using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApplicationPersonBook.AuthData;
using WebApplicationPersonBook.Services;

namespace WebApplicationPersonBook.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IApiAuth _apiAuth { get; }

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            RoleManager<IdentityRole> roleManager,
            IApiAuth apiAuth)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;
            _apiAuth = apiAuth;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }


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

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "Пароль и подтверждение пароля не совпадают.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

            // создание ролей администратора и пользователя
            if (!await _roleManager.RoleExistsAsync(AppConst.AdminRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(AppConst.AdminRole));
                await _roleManager.CreateAsync(new IdentityRole(AppConst.UserRole));
            }
        }
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            var unicEmail = _userManager.FindByEmailAsync(Input.Email).Result;
            if (unicEmail != null)
            {
                ViewData["Message"] = "Пользователь с таким Email уже существует.";
                return Page();
            }

            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email};
                var apiUser = new ApiUser() { Name = Input.Email, Password = Input.Password };

                var apiAnswer = await _apiAuth.ApiUserValidation("register", apiUser);
                if (apiAnswer.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ViewData["Message"] = apiAnswer.ReasonPhrase;
                    return Page();
                }
                var token = apiAnswer.Content.ReadAsStringAsync().Result;
                if(token != null)
                {
                    HttpContext.Session.SetString("Token", token);
                }

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    if (_userManager.Users.Count() == AppConst.FirstRegistration)
                    {
                        // первая регистрация, юзер получает роль админа
                        await _userManager.AddToRoleAsync(user, AppConst.AdminRole);
                    }
                    else if (User.IsInRole(AppConst.AdminRole))
                    {
                        // админ под своим аккаунтом пытается создать нового пользователя
                        await _userManager.AddToRoleAsync(user, AppConst.AdminRole);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, AppConst.UserRole);
                    }

                    // здесь происходит проверка является ли тот кто регистрирует новый аккаунт админом
                    if (!User.IsInRole(AppConst.AdminRole))
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                    }
                    else
                    {
                        return RedirectToPage("/Contacts");
                    }
                    return LocalRedirect(returnUrl);

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Page();
        }
    }
}
