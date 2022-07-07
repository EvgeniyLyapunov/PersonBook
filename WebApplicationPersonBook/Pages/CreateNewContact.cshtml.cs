using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebApplicationPersonBook.Models;

namespace WebApplicationPersonBook.Pages
{
    public class CreateNewContactModel : PageModel
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly HttpClient _httpClient;

        [BindProperty]
        public Contact Contact { get; set; }
        [BindProperty]
        [DisplayName("Image - (no more 100kB size)")]
        public IFormFile Avatar { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool FileSize { get; set; }

        public CreateNewContactModel(IWebHostEnvironment webHostEnvironment,
            UserManager<IdentityUser> userManager,
            HttpClient httpClient)
        {
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _httpClient = httpClient;
            Contact = new Contact();
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Avatar != null && Avatar.Length > AppConst.MaxSizeOfPicture)
            {
                FileSize = true;
                return Page();
            }
            if(ModelState.IsValid)
            {
                Contact.PictureName = Guid.NewGuid().ToString();
                byte[] imageData = null;

                if (Avatar != null)
                {
                    using (var binaryReader = new BinaryReader(Avatar.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)Avatar.Length);
                    }
                    Contact.Avatar = imageData;
                }
                else
                {
                    Contact.Avatar = AppConst.GetDefaultAvatar(_webHostEnvironment.WebRootPath);
                }

                Contact.UserId = User.Identity.Name;

                var url = $@"{AppConst.ApiPath}/api/Values/setdata";

                using (_httpClient)
                {
                    _httpClient.BaseAddress = new Uri(url);
                    var token = HttpContext.Session.GetString("Token");
                    if (!string.IsNullOrEmpty(token))
                    {
                        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    }
                    var content = new StringContent(JsonConvert.SerializeObject(Contact),
                    Encoding.UTF8, "application/json");
                    HttpResponseMessage result = await _httpClient.PostAsync(url, content);

                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return RedirectToPage("/Contacts");
                    }
                }
            }
            return Page();
        }
    }
}
