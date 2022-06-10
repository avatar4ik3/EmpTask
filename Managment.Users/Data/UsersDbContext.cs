using Managment.Users.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Managment.Users.Data;

public class UsersDbContext : IdentityDbContext
{
    public UsersDbContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<RefreshToken> RefreshTokens {get;set;}
}