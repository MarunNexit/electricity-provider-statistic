using System.ComponentModel.DataAnnotations;

namespace electricity_provider_server_api.Models
{
    public class ProviderRating
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ProviderId { get; set; }

        public DateTime Date { get; set; }

        [Range(0, 5)]
        public double Rating { get; set; }

        public bool IsActive { get; set; }

        public User User { get; set; }
    }
}
