
using System.ComponentModel.DataAnnotations;

namespace electricity_provider_server_api.DTOs
{

    public class ChoiceProviderDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ChoiceUserAddressDto
    {
        public int? UserId { get; set; }
        public string? Region { get; set; }
        public string? Street { get; set; }
        public string? ApartmentNumber { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Postcode { get; set; }
    }


    public class GetUserProviderChoiceDto
    {
        public int UserAddressId { get; set; }
        public ChoiceProviderDto Provider { get; set; }
        public ChoiceUserAddressDto Address { get; set; }
        public DateTime ChoiceDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public int? TariffId { get; set; }
    }

    public class GetUserAddressDto
    {
        public int? UserId { get; set; }
        public string? Region { get; set; }
        public string? Street { get; set; }
        public string? ApartmentNumber { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Postcode { get; set; }
    }

    public class GetUserProviderChoiceDtoWithId : GetUserProviderChoiceDto
    {
        public int Id { get; set; }
    }


    public class UserProviderChoiceDto
    {
        public int UserAddressId { get; set; }

        public int ProviderId { get; set; }

        public DateTime ChoiceDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public int? TariffId { get; set; }
    }

    public class UserProviderChoiceDtoWithId : UserProviderChoiceDto
    {
        public int Id { get; set; }
    }

    public class ProviderUserCountDto
    {
        public int Id { get; set; }
        public string ProviderName { get; set; } = string.Empty;
        public int UserCount { get; set; }
    }
}
