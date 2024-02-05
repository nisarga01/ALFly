using System.ComponentModel.DataAnnotations;
using System.Security;

namespace ALFly.Models
{
    public class AgentPermission
    {
        
        public int Id { get; set; }
        public Agents Agent { get; set; }

        // Other properties...
        
        public int PermissionId { get; set; }
        public Permissions Permission { get; set; }
    }
}
