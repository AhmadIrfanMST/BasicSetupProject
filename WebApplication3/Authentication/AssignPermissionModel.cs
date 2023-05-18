using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Authentication
{
    public class AssignPermissionModel
    {
        [Required(ErrorMessage = "Role Id is required")]
        public string roleId { get; set; }

        [Required(ErrorMessage = "Permissions are Required")]
        public List<string> permissions { get; set; }
    }
}
