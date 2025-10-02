using Microsoft.AspNetCore.Identity;

namespace Task5.Database.Entities
{
    public class UserEntity : IdentityUser
    {
        public required string FullName { get; set; }
        public string? Position { get; set; }
        public int UserStatus { get; set; }
        public DateTime LastLoginTime { get; set; }
        public DateTime RegisterTime { get; set; }
    }
}