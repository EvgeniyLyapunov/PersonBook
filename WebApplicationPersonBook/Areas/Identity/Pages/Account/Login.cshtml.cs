using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WebApplicationPersonBook.Services;
using WebApplicationPersonBook.AuthData;
using Microsoft.AspNetCore.Http;

namespace WebApplicationPersonBook.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        private IApiAuth _apiAuth { get; }

        public LoginModel(SignInManager<IdentityUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<IdentityUser> userManager,
            IApiAuth apiAuth)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _apiAuth = apiAuth; 
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var apiUser = new ApiUser() { Name = Input.Email, Password = Input.Password };
                var apiAnswer = await _apiAuth.ApiUserValidation("login", apiUser);
                if (apiAnswer.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ViewData["Message"] = apiAnswer.ReasonPhrase;
                    return Page();
                }
                var token = apiAnswer.Content.ReadAsStringAsync().Result;
                if (token != null)
                {
                    HttpContext.Session.SetString("Token", token);
                }

                // Если регистрация в Web Api произошла из Wpf - desktop клиента
                if(token != null && _userManager.FindByEmailAsync(Input.Email).Result == null)
                {
                    var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                    var regResult = await _userManager.CreateAsync(user, Input.Password);
                    if (_userManager.Users.Count() == AppConst.FirstRegistration)
                    {
                        // первая регистрация, юзер получает роль админа
                        await _userManager.AddToRoleAsync(user, AppConst.AdminRole);
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToPage("/Contacts");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, AppConst.UserRole);
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToPage("/Contacts");
                    }
                }

                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {

                    return RedirectToPage("/Contacts");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }
            return Page();
        }
    }
}
