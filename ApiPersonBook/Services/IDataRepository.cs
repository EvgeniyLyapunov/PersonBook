using DataBaseLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiPersonBook.Services
{
    public interface IDataRepository
    {
        Task CreateData(Contact contact);
        Task DeleteData(int id);
        Task DeleteAllUserData(string UserName);
        Task UpdateData(Contact contact);
        Task<Contact> GetData(int id);
        Task<List<Contact>> GetListData();
        Task<List<Contact>> GetUserListData(string UserId);
    }
}
