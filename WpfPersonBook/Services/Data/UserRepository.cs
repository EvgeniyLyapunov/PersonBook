using Newtonsoft.Json;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WpfPersonBook.Models;

namespace WpfPersonBook.Services.Data
{
    public class UserRepository : IUserRepository
    {
        public async Task<HttpResponseMessage> LoginUser(ApplicationUser user)
        {
            var url = $@"{AppConst.ApiPath}/api/Auth/login";

            using (var _httpClient = new HttpClient())
            {
                _httpClient.BaseAddress = new Uri(url);
                StringContent content = new StringContent(JsonConvert.SerializeObject(user),
                            Encoding.UTF8, "application/json");

                return await _httpClient.PostAsync(url, content);
            }
        }

        public async Task<HttpResponseMessage> RegisterUser(ApplicationUser user)
        {
            var url = $@"{AppConst.ApiPath}/api/Auth/register";

            using (var _httpClient = new HttpClient())
            {
                _httpClient.BaseAddress = new Uri(url);

                StringContent content = new StringContent(JsonConvert.SerializeObject(user),
                            Encoding.UTF8, "application/json");

                return await _httpClient.PostAsync(url, content);
            }
        }
    }
}
