using System.ComponentModel.DataAnnotations;

namespace ALFly.Models
{
    public class Permissions
    {
        [Key]
        public int PermissionId { get; set; }
        public string Permission { get; set; }
        public ICollection<AgentPermission> AgentPermissions { get; set; }

        // Other properties...

    }
}
