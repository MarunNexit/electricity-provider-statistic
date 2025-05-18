using System.ComponentModel.DataAnnotations;

namespace electricity_provider_server_api.Models
{
    public class UserProviderChoice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserAddressId { get; set; }

        [Required]
        public int ProviderId { get; set; }

        public DateTime ChoiceDate { get; set; }

        public DateTime? ContractEndDate { get; set; }

        public int? TariffId { get; set; }

        public UserAddress UserAddress { get; set; }
        public Provider Provider { get; set; }
        public Tariff Tariff { get; set; }
    }
}
