using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;

namespace Talabat.Api.Extensions;

public static class UserManagerExtensions
{
    public static async Task<AppUser?> GetUserWithAddressAsync(this UserManager<AppUser> UserManager, ClaimsPrincipal User)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        var currentUser = await UserManager.Users.Include(U => U.Address).Where(U => U.Email == email)
            .FirstOrDefaultAsync();
        return currentUser;
    }
}
