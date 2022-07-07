using DataBaseLibrary;
using DataBaseLibrary.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ApiPersonBook.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CreateUser(ApplicationUser applicationUser)
        {
            await Task.Run(() => _context.ApplicationUser.AddAsync(applicationUser))
                .ContinueWith((r) => _context.SaveChanges());
        }

        public async Task DeleteUser(int id)
        {
            var applicationUser = await _context.ApplicationUser.FindAsync(id);
            if (applicationUser != null)
            {
                await Task.Run(() =>
                _context.ApplicationUser.Remove(applicationUser))
                    .ContinueWith((r) => _context.SaveChanges());
            }
        }

        public async Task<ApplicationUser> GetUser(ApplicationUser applicationUser)
        {
                var user = await Task.Run(() => _context.ApplicationUser
                                .FirstOrDefault(u => u.Name == applicationUser.Name &&
                                                        u.Password == applicationUser.Password));
                if (user != null) return user;
                else return null;
        }

        public async Task<ApplicationUser> GetUserByName(string Name)
        {
            var user = await Task.Run(() => _context.ApplicationUser
                                .FirstOrDefault(u => u.Name == Name));
            if (user != null) return user;
            else return null;
        }
    }
}
