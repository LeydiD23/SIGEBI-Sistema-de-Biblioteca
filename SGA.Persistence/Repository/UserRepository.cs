using Microsoft.EntityFrameworkCore;
using SGA.Domain.Entitys;
using SGA.Domain.Repository;
using SGA.Persistence.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGA.Persistence.Repository
{
    public class UserRepository
       : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context)
            : base(context)
        {
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Set<User>()
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<User>> GetByRoleAsync(string role)
        {
            return await _context.Set<User>()
                .Where(u => u.Role == role)
                .ToListAsync();
        }

        public async Task<User?> ValidateUserAsync(string username, string password)
        {
            return await _context.Set<User>()
                .FirstOrDefaultAsync(u =>
                    u.Username == username &&
                    u.Password == password);
        }
    }
}
