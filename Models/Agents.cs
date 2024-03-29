﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ALFly.Enums;

namespace ALFly.Models
{
    public class Agents
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; }

        public string? Photo { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }

        //[Required(ErrorMessage = "Password is required")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //[Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        //[NotMapped] // Not stored in the database
        public string ConfirmPassword { get; set; }

        //[Required(ErrorMessage = "Role is required")]
        public Role Role { get; set; }
        // Navigation property for the many-to-many relationship
        public virtual ICollection<AgentPermission> AgentPermissions { get; set; }

    }
}
