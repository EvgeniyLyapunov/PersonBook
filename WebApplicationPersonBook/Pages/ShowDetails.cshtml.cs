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
    public class ShowDetailsModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public ShowDetailsModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty(SupportsGet = true)]
        public Contact Contact { get; set; }

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
        }
    }
}
