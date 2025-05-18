using Swashbuckle.AspNetCore.SwaggerGen;

namespace electricity_provider_server_api.DTOs
{
    public class ElectricityMarketShareDto
    {
        public string Quarter { get; set; }
        public Dictionary<string, double?> SupplierShares { get; set; }
    }

    public class TopSupplierShareDto
    {
        public string Supplier { get; set; }
        public double Share { get; set; }
    }

    public class MarketShareGroupDto
    {
        public string GroupName { get; set; }
        public double Share { get; set; }
    }

    public class ProviderShareDto
    {
        public string Name { get; set; }
        public double Share { get; set; }
    }

    public class MarketShareGroupDetailedDto
    {
        public string GroupName { get; set; }
        public double Share { get; set; }
        public List<ProviderShareDto> Providers { get; set; }
    }
}
