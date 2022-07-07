using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApplicationPersonBook.Models;

namespace WebApplicationPersonBook.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DeleteModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty(SupportsGet = true)]
        public Contact Contact { get; set; }

        public async Task OnGetAsync(int id)
        {
            var url = @$"http://localhost:63810/api/Values/{id}";
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
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var url = @$"{AppConst.ApiPath}/api/Values/deldata/{Contact.Id}";

            using (_httpClient)
            {
                _httpClient.BaseAddress = new Uri(url);
                var token = HttpContext.Session.GetString("Token");
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                }
                HttpResponseMessage result = await _httpClient.DeleteAsync(url);

                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return RedirectToPage("/Contacts");
                }
            }
            return Page();
        }
    }
}
