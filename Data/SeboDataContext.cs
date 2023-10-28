using Microsoft.EntityFrameworkCore;
using SeboOnline.Data.Mappings;
using SeboOnline.Models;

namespace SeboOnline.Data;

public class SeboDataContext : DbContext
{
    public DbSet<User> Users { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder options)
    //    => options.UseSqlServer();
    public SeboDataContext(DbContextOptions<SeboDataContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserMap());
    }
}