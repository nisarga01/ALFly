using ALFly.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ALFly.DTO.AgentRequestDTO
{
    public class AgentRequestDTO
    {
        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; }

        public IFormFile? Photo { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public Role Role { get; set; }
    }
}