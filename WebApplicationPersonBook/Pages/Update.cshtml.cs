using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApplicationPersonBook.Models;

namespace WebApplicationPersonBook.Pages
{
    public class UpdateModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public UpdateModel(HttpClient httpClient)
        {
            _httpClient = httpClient;   
        }

        [BindProperty(SupportsGet = true)]
        public Contact Contact { get; set; }
        [BindProperty]
        [DisplayName("Image - (no more 100kB size)")]
        public IFormFile Avatar { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool FileSize { get; set; }




        public async Task OnGetAsync(int id)
        {
            var url = @$"{AppConst.ApiPath}/api/Values/{id}";
            _httpClient.BaseAddress = new Uri(url);
            var token = HttpContext.Session.GetString("Token");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
            else
            {
                RedirectToPage("Identity/Account/Login");
            }

            string json = await _httpClient.GetStringAsync(url);
            Contact = JsonConvert.DeserializeObject<Contact>(json);
            HttpContext.Session.Set("Avatar", Contact.Avatar);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Contact.Avatar = HttpContext.Session.Get("Avatar");
            if (Avatar != null && Avatar.Length > AppConst.MaxSizeOfPicture)
            {
                FileSize = true;
                return Page();
            }

            if (ModelState.IsValid)
            {
                if (Avatar != null)
                {
                    Contact.PictureName = Guid.NewGuid().ToString();
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(Avatar.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)Avatar.Length);
                    }
                    Contact.Avatar = imageData;
                }

                var url = $@"{AppConst.ApiPath}/api/Values/updata";
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
