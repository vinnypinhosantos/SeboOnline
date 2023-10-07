using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace SeboOnline.Models;

// Feito baseado no seguinte repositório: https://github.com/balta-io/2808/
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public bool IsActive { get; set; }
    public IList<Role> Roles { get; set; }
}