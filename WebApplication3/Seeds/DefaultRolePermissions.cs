using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WebApplication3.Authentication;
using WebApplication3.Models;

namespace WebApplication3.Seeds
{
    public class DefaultRolePermissions
    {
        public static async Task SeedRolePermissionsAsync(RoleManager<IdentityRole> roleManager,string role_name, MyDbContext dbcontext)
        {
            var role = await roleManager.FindByNameAsync(role_name);
            if (role != null){
                var allPermissions = Permissions.getPermissions(role.Name);
                var allClaims = await roleManager.GetClaimsAsync(role);
                foreach (var permission in allPermissions)
                {
                    var perm = dbcontext.permissions.Where(p => p.Name == permission);

                    if (perm != null && !allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
                    {
                        await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                    }
                }
            }
        }
    }
}
