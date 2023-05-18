using System.Data;
using WebApplication3.Models;

namespace WebApplication3.Seeds
{
    public static class PermissionSeeder
    {
        public static async Task SeedPermissionsAsync(MyDbContext dbContext)
        {
            if (!dbContext.permissions.Any())
            {
                //List<Permission> ab =  new List<WebApplication3.Models.Permission>
                // Define your predefined permissions
                var permissions = new List<string>
                {
                    "Permissions.Dashboard_ViewMap",
                    "Permissions.GeoFencing_Create",
                    "Permissions.GeoFencing_View",
                    "Permissions.GeoFencing_Edit",
                    "Permissions.GeoFencing_Delete",
                    "Permissions.Devices_Add",
                    "Permissions.Devices_View",
                    "Permissions.Devices_Edit",
                    "Permissions.Devices_Delete",
                    "Permissions.Devices_ApplyGeoFence",
                    "Permissions.Devices_ApplyConfiguration",
                    "Permissions.Devices_SetupAlerts",
                    "Permissions.Configuration_Create",
                    "Permissions.Configuration_View",
                    "Permissions.Configuration_Edit",
                    "Permissions.Configuration_Delete",
                    "Permissions.Logs",
                    "Permissions.Notifications",
                    "Permissions.UserManagement_EditProfile",
                    "Permissions.UserManagement_ChangePassword",
                };

                foreach (var permissionName in permissions)
                {
                    var permission = new WebApplication3.Models.Permission(permissionName);
                    dbContext.permissions.Add(permission);
                }

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
