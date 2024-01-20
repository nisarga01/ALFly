using ALFly.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ALFly.DTO.AgentResponseDTO
{
    public class AgentResponseDTO
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public byte[]? Photo { get; set; } // Use byte[] to store image data

        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        [NotMapped] // Not stored in the database
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public Role Role { get; set; }
    }
}
