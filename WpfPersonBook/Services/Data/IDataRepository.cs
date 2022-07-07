using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WpfPersonBook.Models;

namespace WpfPersonBook.Services.Data
{
    public interface IDataRepository
    {
        Task<List<WpfContact>> GetListData();
        Task<HttpResponseMessage> CreateData(Contact contact, string token);
        Task<HttpResponseMessage> UpdateData(Contact contact, string token);
        Task<HttpResponseMessage> DeleteData(int id, string token);
    }
}
