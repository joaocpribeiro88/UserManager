using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UserManager.Domain.Entities;

namespace UserManager.Infrastructure.Data;
public class UserManagerDbContext : DbContext
{
    public UserManagerDbContext(DbContextOptions<UserManagerDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Employment> Employments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetCallingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
