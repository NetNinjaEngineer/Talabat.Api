using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity;

public static class AppIdentityDbContextSeed
{
    public static async Task SeedUserAsync(UserManager<AppUser> userManager)
    {
        if (!userManager.Users.Any())
        {
            var appUser = new AppUser
            {
                DisplayName = "Mohamed Elhelaly",
                Email = "memo5260287@outlook.com",
                UserName = "mohamed.elhelaly12",
                PhoneNumber = "01145753861"
            };

            await userManager.CreateAsync(appUser, "P@ssword123");
        }
    }
}
