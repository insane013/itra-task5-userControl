using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task5.Database.Entities;

namespace Task5.Database.Repositories
{
    public interface IUserRepository
    {
        public Task<IEnumerable<UserEntity>> GetUsers();
    }
}