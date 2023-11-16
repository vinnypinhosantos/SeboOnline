using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace SeboOnline.Models;

// Feito baseado no seguinte repositório: https://github.com/balta-io/2808/
public class User
{
    public User()
    {
        Roles = new List<Role>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public bool IsActive { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? Specialization { get; set; }
    public IList<Role> Roles { get; set; }
    public int UserId => Id;
    public bool IsAdministrator()
    {
        return Roles.Any(role => role.Name == "admin");
    }
    public void AddRole(string roleName)
    {
        if (Roles == null)
        {
            Roles = new List<Role>();
        }
        if (!Roles.Any(role => role.Name == roleName))
        {
            Roles.Add(new Role { Name = roleName });
        }
    }
}