using System.ComponentModel.DataAnnotations;

namespace electricity_provider_server_api.Models
{
    public class Provider
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Url]
        public string? Website { get; set; }

        public string? Logo { get; set; }

        public ICollection<ProviderAddress> Addresses { get; set; }

        public ICollection<ProviderRating> ProviderRatings { get; set; }
    }
}