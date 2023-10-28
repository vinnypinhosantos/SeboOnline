using System.Security.Claims;
using SeboOnline.Models;

namespace SeboOnline.Extensions;

public static class RoleClaimsExtension
{
    public static IEnumerable<Claim> GetClaims(this User user)
    {
        var result = new List<Claim>()
        {
            new (ClaimTypes.Name, user.Email)
        };
        result.AddRange(
            user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name))
        );
        return result;
    }
    public static int GetUserId(this User user)
    {
        var userIdClaim = GetClaims(user).FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);

        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
        {
            return userId;
        }

        return 0;
    }
}