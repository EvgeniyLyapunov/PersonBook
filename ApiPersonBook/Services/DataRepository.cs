using DataBaseLibrary;
using DataBaseLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPersonBook.Services
{
    public class DataRepository : IDataRepository
    {
        private readonly ApplicationDbContext _context;
        public DataRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateData(Contact contact)
        {
            var validObj = await _context.Contact.FirstOrDefaultAsync(u => u.Id == contact.Id);
            if (validObj == null)
            {
                await Task.Run(() => _context.Contact.Add(contact))
                .ContinueWith((r) => _context.SaveChanges());
            }
        }

        public async Task UpdateData(Contact contact)
        {
            await Task.Run(() => _context.Contact.Update(contact))
                .ContinueWith((r) => _context.SaveChanges());
        }

        public async Task DeleteData(int id)
        {
            var validObj = await _context.Contact.FirstOrDefaultAsync(u => u.Id == id); ;
            if (validObj != null)
            {
                await Task.Run(() => _context.Contact.Remove(validObj))
                .ContinueWith((r) => _context.SaveChanges());
            }
        }

        public async Task DeleteAllUserData(string UserName)
        {
            var data = await _context.Contact.Where(u => u.UserId == UserName).ToListAsync();

            if (data.Count != 0)
            {
                await Task.Run(() => _context.Contact.RemoveRange(data))
                .ContinueWith((r) => _context.SaveChanges());
            }
        }


        public async Task<Contact> GetData(int id)
        {
            return await Task.Run(() => _context.Contact.FirstOrDefaultAsync(u => u.Id == id));
        }

        public async Task<List<Contact>> GetListData()
        {
            return await Task.Run(() => _context.Contact.ToListAsync());    
        }

        public async Task<List<Contact>> GetUserListData(string UserId)
        {
            return await Task.Run(() => _context.Contact.Where(u => u.UserId == UserId).ToList());
        }

    }
}
