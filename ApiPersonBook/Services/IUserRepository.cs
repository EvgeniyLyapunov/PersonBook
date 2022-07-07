using DataBaseLibrary.Models;
using System.Threading.Tasks;

namespace ApiPersonBook.Services
{
    public interface IUserRepository
    {
        Task CreateUser(ApplicationUser applicationUser);
        Task DeleteUser(int id);
        Task<ApplicationUser> GetUser(ApplicationUser applicationUser);
        Task<ApplicationUser> GetUserByName(string Name);
    }
}
