using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApplicationPersonBook.AuthData;

namespace WebApplicationPersonBook.Services
{
    public class ApiAuth : IApiAuth
    {
        private HttpClient _httpClient { get; }
        public ApiAuth(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> ApiUserValidation(string endOfPath, ApiUser apiUser)
        {
            var url = @$"{AppConst.ApiPath}/api/Auth/{endOfPath}";

                _httpClient.BaseAddress = new Uri(url);
                StringContent content = new StringContent(JsonConvert.SerializeObject(apiUser),
                        Encoding.UTF8, "application/json");
                return await _httpClient.PostAsync(url, content);
        }

        public async Task<HttpResponseMessage> ApiUserCreation(string endOfPath, ApiUser apiUser, string token)
        {
            var url = @$"{AppConst.ApiPath}/api/Auth/{endOfPath}";

                _httpClient.BaseAddress = new Uri(url);
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                StringContent content = new StringContent(JsonConvert.SerializeObject(apiUser),
                        Encoding.UTF8, "application/json");
                return await _httpClient.PostAsync(url, content);
        }

        public async Task<HttpResponseMessage> ApiUserDelete(string endOfPath, string userName, string token)
        {
            var url = @$"{AppConst.ApiPath}/api/Auth/{endOfPath}";

            _httpClient.BaseAddress = new Uri(url);
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            StringContent content = new StringContent(JsonConvert.SerializeObject(userName),
                        Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(url, content);
        }
    }
}
