using Microsoft.AspNetCore.Identity;

namespace Task5.Database.Entities
{
    public class UserEntity : IdentityUser
    {
        public int userStatus;
        public DateTime lastLoginTime;
    }
}