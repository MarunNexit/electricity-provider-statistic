using System.ComponentModel.DataAnnotations;

namespace electricity_provider_server_api.Models
{
    public class Tariff
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProviderId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public bool IsAvailable { get; set; }

        public Provider Provider { get; set; }
    }
}
