using System.ComponentModel.DataAnnotations;
using System.Net;

namespace electricity_provider_server_api.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }
        public string? Name { get; set; }
        public string? Icon { get; set; }

        public string PasswordHash { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public ICollection<UserAddress> Addresses { get; set; }
    }
}