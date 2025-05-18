namespace electricity_provider_server_api.DTOs
{
    public class ProviderBasicDto
    {
        public string Name { get; set; }
        public string Website { get; set; }
        public string Logo { get; set; }
    }

    public class ProviderDto : ProviderBasicDto
    {
        public List<ProviderAddressDto> Addresses { get; set; }
    }

    public class ProviderWithIdDto : ProviderBasicDto
    {
        public int Id { get; set; }
        public List<ProviderAddressWithIdDto> Addresses { get; set; }
    }

    public class ProviderAddressDto
    {
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string? Postcode { get; set; }
    }

    public class ProviderRatingDto
    {
        public int UserId { get; set; }
        public int ProviderId { get; set; }
        public double Rating { get; set; }
    }

    public class ProviderWithRatingDto : ProviderWithIdDto
    {
        public double AverageRating { get; set; }
    }

    public class ProviderWithRatingAndIdDto : ProviderWithRatingDto
    {
        public int Id { get; set; }
    }

    public class ProviderAddressWithIdDto : ProviderAddressDto
    {
        public int Id { get; set; }
    }


    public class ProviderStatsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Website { get; set; }
        public string Logo { get; set; }

        public double? LastShare { get; set; }
        public double? AverageYear { get; set; }
        public double? Peak { get; set; }
        public double? Min { get; set; }
        public int? Since { get; set; }

        public double? AverageRating { get; set; }
    }
}
