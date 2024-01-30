using ALFly.Enums;
using System.ComponentModel.DataAnnotations;

namespace ALFly.DTO.EditAgentDetailsDTO
{
    public class EditAgentDetailsRequestDTO
    {
        public string FullName { get; set; }

        public IFormFile? Photo { get; set; }

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
