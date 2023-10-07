﻿using System.Security.Claims;
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
}