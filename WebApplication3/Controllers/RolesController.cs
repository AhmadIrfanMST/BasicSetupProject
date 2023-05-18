using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Security.Claims;
using System.Text.Json;
using WebApplication3.Authentication;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        MyDbContext dbcontext;
        public RolesController(RoleManager<IdentityRole> roleManager, MyDbContext context)
        {
            _roleManager = roleManager;
            dbcontext = context;    
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(new Response { Status = "Success", Message = "Roles List", data = roles });
            //return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (roleName != null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
            }
            return Ok(new Response { Status = "Success", Message = "Role Created Successfully" });
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        [Route("add-new-role")]
        public async Task<IActionResult> addNewRole([FromBody] JsonElement entity)
        {
            dynamic json = JsonConvert.DeserializeObject(entity.ToString());
            if (!await _roleManager.RoleExistsAsync(json.roleName.ToString()))
                await _roleManager.CreateAsync(new IdentityRole(json.roleName.ToString()));
            return Ok(new Response { Status = "Success", Message = "Role Created Successfully" });
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        [Route("assign-permission-to-role")]
        public async Task<IActionResult> assignPermissionToRole([FromForm] AssignPermissionModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.roleId);
            if (role != null && model.permissions.Count() > 0)
            {
                var allClaims = await _roleManager.GetClaimsAsync(role);
                var allPermissions = Permissions.getPermissions(role.Name);// these are my permissions to assign

                foreach (var permissionName in model.permissions)
                {
                    var perm = dbcontext.permissions.Where(p => p.Name == permissionName);

                    if (perm != null && !allClaims.Any(a => a.Type == "Permission" && a.Value == permissionName))
                    {
                        await _roleManager.AddClaimAsync(role, new Claim("Permission", permissionName));
                        return Ok(new Response { Status = "Success", Message = "Permissions Assigned Successfully" });
                    }
                    else
                    {
                        return Ok(new Response { Status = "Failed", Message = "Unable to assign permission, Please try again" });
                    }
                }
                return Ok(new Response { Status = "Failed", Message = "Unable to process, please check your data" });
            }
            else
                return Ok(new Response { Status = "Failed", Message = "Unable to process, please check your data" });

        }
        /* public async Task<IActionResult> removePermissionToRole([FromForm] AssignPermissionModel model)
         {
             var role = await roleManager.FindByIdAsync(model.roleId);
             if (role != null && model.permissions.Count() > 0)
             {
                 var allClaims = await roleManager.GetClaimsAsync(role);
                 var allPermissions = Permissions.getPermissions(role.Name);// these are my permissions to assign

                 foreach (var permissionName in model.permissions)
                 {
                     var perm = dbContext.permissions.Where(p => p.Name == permissionName);

                     if (perm != null && !allClaims.Any(a => a.Type == "Permission" && a.Value == permissionName))
                     {
                         await roleManager.AddClaimAsync(role, new Claim("Permission", permissionName));
                         return Ok(new Response { Status = "Success", Message = "Permissions Assigned Successfully" });
                     }
                     else
                     {
                         return Ok(new Response { Status = "Failed", Message = "Unable to assign permission, Please try again" });
                     }
                 }
                 return Ok(new Response { Status = "Failed", Message = "Unable to process, please check your data" });
             }
             else
                 return Ok(new Response { Status = "Failed", Message = "Unable to process, please check your data" });

         }*/
    }
}
