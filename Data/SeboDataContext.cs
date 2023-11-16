using Microsoft.EntityFrameworkCore;
using SeboOnline.Data.Mappings;
using SeboOnline.Models;

namespace SeboOnline.Data;

public class SeboDataContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Category> Categories { get; set; }

    public DbSet<Transaction> Transactions { get; set; }
    //protected override void OnConfiguring(DbContextOptionsBuilder options)
    //    => options.UseSqlServer();
    public SeboDataContext(DbContextOptions<SeboDataContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserMap());
        modelBuilder.ApplyConfiguration(new RoleMap());
        modelBuilder.ApplyConfiguration(new ItemMap());
        modelBuilder.ApplyConfiguration(new CategoryMap());
        modelBuilder.ApplyConfiguration(new TransactionMap());
    }
}