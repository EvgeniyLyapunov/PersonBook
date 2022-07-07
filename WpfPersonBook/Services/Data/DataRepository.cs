using AutoMapper;
using Newtonsoft.Json;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WpfPersonBook.Models;
using WpfPersonBook.Services.Mapping;

namespace WpfPersonBook.Services.Data
{
    public class DataRepository : IDataRepository
    {
        private readonly IMapper _mapper;

        public DataRepository()
        {
            _mapper = Locator.Current.GetService<IMapper>();
        }

        public async Task<List<WpfContact>> GetListData()
        {
            string url = $@"{AppConst.ApiPath}/api/Values/getlist";

            using (var _httpClient = new HttpClient())
            {
                string json = await _httpClient.GetStringAsync(url);
                var ApiContacts = JsonConvert.DeserializeObject<List<Contact>>(json);

                var WpfContacts = new List<WpfContact>();
                foreach (var item in ApiContacts)
                {
                    WpfContacts.Add(_mapper.Map<WpfContact>(item));
                }
                return WpfContacts;
            }
        }
        public async Task<HttpResponseMessage> CreateData(Contact contact, string token)
        {
            var url = $@"{AppConst.ApiPath}/api/Values/setdata";

            using (var _httpClient = new HttpClient())
            {
                _httpClient.BaseAddress = new Uri(url);

                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                }

                var content = new StringContent(JsonConvert.SerializeObject(contact),
                            Encoding.UTF8, "application/json");

                return await _httpClient.PostAsync(url, content);
            }
        }
        public async Task<HttpResponseMessage> UpdateData(Contact contact, string token)
        {
            var url = $@"{AppConst.ApiPath}/api/Values/updata";

            using (var _httpClient = new HttpClient())
            {
                _httpClient.BaseAddress = new Uri(url);

                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                }

                var content = new StringContent(JsonConvert.SerializeObject(contact),
                            Encoding.UTF8, "application/json");

                return await _httpClient.PostAsync(url, content);
            }
        }
        public async Task<HttpResponseMessage> DeleteData(int id, string token)
        {
            var url = $@"{AppConst.ApiPath}/api/Values/deldata/{id}";

            using(var _httpClient = new HttpClient())
            {
                _httpClient.BaseAddress = new Uri(url);

                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                }

                return await _httpClient.DeleteAsync(url);
            }
        }
    }
}
