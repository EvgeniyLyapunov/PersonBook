using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebApplicationPersonBook.Models;

namespace WebApplicationPersonBook.Pages
{
    public class ContactsModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public ContactsModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            Contacts = new List<Contact>();
        }

        [BindProperty(SupportsGet = true)]
        public List<Contact> Contacts { get; set; }
        public async Task OnGetAsync()
        {
            var url = @$"{AppConst.ApiPath}/api/Values/getlist";

            string json = await _httpClient.GetStringAsync(url);

            Contacts = JsonConvert.DeserializeObject<List<Contact>>(json);
        }
    }
}
