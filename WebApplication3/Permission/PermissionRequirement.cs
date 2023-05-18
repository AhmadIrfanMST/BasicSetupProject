using Microsoft.AspNetCore.Authorization;

namespace WebApplication3.Permission
{
    internal class PermissionRequirement: IAuthorizationRequirement
    {
        public string Permission { get; private set; }
        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }
}
