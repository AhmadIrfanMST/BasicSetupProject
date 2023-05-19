using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Reflection;
using System.Security.Claims;
using WebApplication3.Models;
using WebApplication3.ViewModels;

namespace WebApplication3.helpers
{
    public static class ClaimsHelper
    {
        public static async Task<bool> GetPermissionsByRoleAsync(this RoleManager<IdentityRole> _roleManager,UserManager<IdentityUser> _userManager , string permissionToCheck,string userString)
        {
            if (userString != null)
            {
                IdentityUser loginUser = JsonConvert.DeserializeObject<IdentityUser>(userString);
                var roleNames = await _userManager.GetRolesAsync(loginUser);
                if (roleNames == null)
                {
                    return false;
                }

                var roles = new List<IdentityRole>();
                foreach (var name in roleNames)
                {
                    var role = await _roleManager.FindByNameAsync(name);
                    if (role != null)
                    {
                        roles.Add(role);
                    }
                }

                foreach (var roleobj in roles)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(roleobj);
                    var permissionClaims = roleClaims.Where(c => c.Type == "Permission" && c.Value == permissionToCheck).ToList();

                    if (permissionClaims.Any())
                    {
                        return true;
                    }
                }

            }
            return false;
        }
    }
}
