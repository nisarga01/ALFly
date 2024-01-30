using ALFly.Enums;
using System.ComponentModel.DataAnnotations;

namespace ALFly.DTO.EditAgentDetailsDTO
{
    public class EditAgentResponseDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        public byte[]? Photo { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public Role Role { get; set; }
    }
}
