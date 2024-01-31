using ALFly.Enums;
using System.ComponentModel.DataAnnotations;

namespace ALFly.DTO.GetAgentDetailsDTO
{
    public class GetAgentDetailsDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; }

        public string? Photo { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public Role Role { get; set; }
    }
}
