using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class Permission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Permission(string name)
        {
            Name = name;
        }

        //public string PermissionId { get; set; }    
        public string Name { get; set; }    
    }
}
