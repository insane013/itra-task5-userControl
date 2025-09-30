using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Task5.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Task5.Database.DbContext
{
    public class UserDbContext : IdentityDbContext<UserEntity, IdentityRole, string>
{
    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        _ = builder?.Entity<UserEntity>();
    }
}
}