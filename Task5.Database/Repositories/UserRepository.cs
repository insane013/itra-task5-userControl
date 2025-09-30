using Microsoft.EntityFrameworkCore;
using Task5.Database.DbContext;
using Task5.Database.Entities;

namespace Task5.Database.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(UserDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<UserEntity>> GetUsers()
        {
            return await this.context.Users.AsNoTracking().ToListAsync();
        }
    }
}