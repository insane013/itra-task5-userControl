using Task5.Database.DbContext;

namespace Task5.Database.Repositories
{
    public abstract class BaseRepository
    {
        protected UserDbContext context;

        protected BaseRepository(UserDbContext context)
        {
            this.context = context;
        }
    }
}