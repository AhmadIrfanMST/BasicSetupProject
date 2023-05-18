using System.Data;

namespace WebApplication3.Authentication
{
    public static class Permissions
    {
        public const string AssignRole = "AssignRole";
        public const string AddRole = "AddRole";
        public const string RemoveRole = "RemoveRole";
        public const string EditRole = "EditRole";
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.Create",
                $"Permissions.{module}.View",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete",
            };
        }
        public static List<string> getPermissions(string roleName)
        {
            List<string> userPermissions = new List<string>()
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
                    "Permissions.Configuration_View",
                    "Permissions.UserManagement_EditProfile",
                    "Permissions.UserManagement_ChangePassword",
                };
            List<string> adminList = new List<string>()
                {
                    "Permissions.Configuration_Create",
                    "Permissions.Configuration_Edit",
                    "Permissions.Configuration_Delete",
                    "Permissions.Logs",
                    "Permissions.Notifications"
                };
            if (roleName == UserRoles.SuperAdmin)
            {
                adminList.AddRange(userPermissions);
                return adminList;
            }
            else if (roleName == UserRoles.Admin)
            {
                adminList.AddRange(userPermissions);
                return adminList;
            }
            else if (roleName == UserRoles.User)
            {
                return userPermissions;
            }
            else
                return new List<string> (){ };
            
        }
        public static class Products
        {
            public const string View = "Permissions.Products.View";
            public const string Create = "Permissions.Products.Create";
            public const string Edit = "Permissions.Products.Edit";
            public const string Delete = "Permissions.Products.Delete";
        }
    }
}
