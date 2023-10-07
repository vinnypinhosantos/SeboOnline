using Microsoft.EntityFrameworkCore;
using SeboOnline.Data.Mappings;
using SeboOnline.Models;

namespace SeboOnline.Data;

public class SeboDataContext : DbContext
{
    public DbSet<User> Users { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer("Server=localhost,1433;Database=SeboOnline;User ID=sa;Password=Woody327YNWA!;Trusted_Connection=False;TrustServerCertificate=True;");
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserMap());
    }
}