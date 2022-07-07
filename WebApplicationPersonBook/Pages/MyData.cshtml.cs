using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebApplicationPersonBook.Models;

namespace WebApplicationPersonBook.Pages
{
    public class MyDataModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public MyDataModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            Contacts = new List<Contact>();
        }

        [BindProperty(SupportsGet = true)]
        public List<Contact> Contacts { get; set; }

        public async Task OnGetAsync()
        {
            var url = @$"{AppConst.ApiPath}/api/Values/getlist/{User.Identity.Name}";
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

            Contacts = JsonConvert.DeserializeObject<List<Contact>>(json);

        }
    }
}
