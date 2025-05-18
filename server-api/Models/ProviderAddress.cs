using System.ComponentModel.DataAnnotations;

namespace electricity_provider_server_api.Models
{
    public class ProviderAddress
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProviderId { get; set; }

        [Required]
        [StringLength(200)]
        public string Region { get; set; }

        [Required]
        [StringLength(200)]
        public string Street { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; }

        [Required]
        [StringLength(100)]
        public string Country { get; set; }

        [StringLength(200)]
        public string? Postcode { get; set; }

        public Provider Provider { get; set; }
    }
}
