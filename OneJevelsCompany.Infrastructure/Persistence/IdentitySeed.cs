using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace OneJevelsCompany.Infrastructure.Persistence
{
    public static class IdentitySeed
    {
        public static async Task ApplyAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var roleMgr =
                scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var userMgr =
                scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            const string AdminRole = "Admin";

            if (!await roleMgr.RoleExistsAsync(AdminRole))
            {
                var roleResult =
                    await roleMgr.CreateAsync(new IdentityRole(AdminRole));

                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        "Failed to create Admin role: " +
                        string.Join(
                            "; ",
                            roleResult.Errors.Select(e => e.Description)));
                }
            }

            const string email = "admin@onejevels.test";
            const string pwd = "Admin!2345";

            var admin = await userMgr.FindByEmailAsync(email);

            if (admin == null)
            {
                admin = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var create = await userMgr.CreateAsync(admin, pwd);

                if (!create.Succeeded)
                {
                    throw new InvalidOperationException(
                        "Failed to create seed admin user: " +
                        string.Join(
                            "; ",
                            create.Errors.Select(e => e.Description)));
                }
            }

            if (!await userMgr.IsInRoleAsync(admin, AdminRole))
            {
                var addRole =
                    await userMgr.AddToRoleAsync(admin, AdminRole);

                if (!addRole.Succeeded)
                {
                    throw new InvalidOperationException(
                        "Failed to assign Admin role: " +
                        string.Join(
                            "; ",
                            addRole.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}