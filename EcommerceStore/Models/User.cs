using System.ComponentModel.DataAnnotations;

namespace EcommerceStore.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "Customer"; // "Admin" or "Customer"

        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}