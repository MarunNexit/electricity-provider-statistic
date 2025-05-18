using electricity_provider_server_api.DTOs;
using electricity_provider_server_api.Models;
using electricity_provider_server_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using System.Globalization;
using System.Text;

namespace electricity_provider_server_api.ControllersS
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataAnalytics : ControllerBase
    {
        private readonly ProvidersService _providersService;

        public DataAnalytics(ProvidersService providersService)
        {
            _providersService = providersService;
        }

        [HttpGet("average_prices_forward")]
        public async Task<IActionResult> GetAveragePrices()
        {
            var prices = await CsvReaderService.LoadElectricityPricesAsync("AnalyseData/electricity-prices-forwa.csv");
            prices = FilterEverySeven(prices);
            return Ok(prices);
        }

        [HttpGet("electricity-market-shares")]
        public IActionResult GetElectricityMarketShares()
        {
            var data = CsvReaderService.ReadElectricityMarketShares("AnalyseData/electricity-supply-marke.csv");

            return Ok(data);
        }

        [HttpGet("top6-market-shares")]
        public IActionResult GetTop6CurrentMarketShares()
        {
            var data = CsvReaderService.ReadElectricityMarketShares("AnalyseData/electricity-supply-marke.csv");

            if (data == null || data.Count == 0)
                return NotFound("No market share data found.");

            var latestQuarterData = data.Last();

            if (latestQuarterData.SupplierShares == null || latestQuarterData.SupplierShares.Count == 0)
                return NotFound("No supplier shares found for the latest quarter.");

            var top6 = latestQuarterData.SupplierShares
                .Where(s => s.Value.HasValue)
                .OrderByDescending(s => s.Value.Value)
                .Take(6)
                .Select(s => new TopSupplierShareDto
                {
                    Supplier = s.Key,
                    Share = Math.Round(s.Value.Value, 2)
                })
                .ToList();

            return Ok(top6);
        }


        [HttpGet("providers-stats-table")]
        public async Task<IActionResult> GetProviderStatisticsTable([FromQuery] int year = 2023)
        {
            var marketData = CsvReaderService.ReadElectricityMarketShares("AnalyseData/electricity-supply-marke.csv");

            if (marketData == null || marketData.Count == 0)
                return NotFound("No market share data found.");

            var providersFromDb = await _providersService.GetProvidersWithRatingsAsync();

            var latestQuarter = marketData.Last();

            var providerStats = new List<ProviderStatsDto>();

            foreach (var provider in providersFromDb)
            {
                string name = $"\"{provider.Name}\"";

                Console.WriteLine($"🔍 Looking for provider: {name}");
                Console.WriteLine($"    Keys available in first market row: {string.Join(", ", marketData[0].SupplierShares.Keys)}");

                var shares = marketData
                    .Select(m => m.SupplierShares.ContainsKey(name) ? m.SupplierShares[name] : null)
                    .Where(v => v.HasValue)
                    .Select(v => v.Value)
                    .ToList();

                var sharesYear = marketData
                    .Where(m => m.Quarter.Contains(year.ToString()))
                    .Select(m => m.SupplierShares.ContainsKey(name) ? m.SupplierShares[name] : null)
                    .Where(v => v.HasValue)
                    .Select(v => v.Value)
                    .ToList();

                var firstEntry = marketData.FirstOrDefault(q => q.SupplierShares.ContainsKey(name) && q.SupplierShares[name].HasValue);

                int? sinceYear = null;
                if (firstEntry != null && DateTime.TryParseExact(firstEntry.Quarter, "yyyy Q#",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
                {
                    sinceYear = parsedDate.Year;
                }
                else if (firstEntry != null && firstEntry.Quarter.Length >= 4 && int.TryParse(firstEntry.Quarter.Substring(0, 4), out int yearParsed))
                {
                    sinceYear = yearParsed;
                }

                providerStats.Add(new ProviderStatsDto
                {
                    Id = provider.Id,
                    Name = provider.Name,
                    Website = provider.Website,
                    Logo = provider.Logo,

                    LastShare = latestQuarter.SupplierShares.ContainsKey(name)
                                ? latestQuarter.SupplierShares[name]
                                : null,

                    AverageYear = sharesYear.Count > 0 ? Math.Round(sharesYear.Average(), 2) : null,
                    AverageRating = provider.AverageRating > 0 ? provider.AverageRating : 0,
                    Peak = shares.Count > 0 ? Math.Round(shares.Max(), 2) : null,
                    Min = shares.Count > 0 ? Math.Round(shares.Min(), 2) : null,
                    Since = sinceYear
                });
            }

            return Ok(providerStats);
        }

        [HttpGet("market-share-pie")]
        public IActionResult GetMarketSharePieChart([FromQuery] int year = 2024)
        {
            var electricityData = CsvReaderService.ReadElectricityMarketShares("AnalyseData/electricity-supply-marke.csv");
            var grouped = CsvReaderService.GetMarketShareByGroupDetailed(electricityData, year);

            return Ok(grouped);
        }






        [HttpPost("average_prices_forward")]
        public IActionResult PostElectricityPrice([FromBody] ElectricityPriceDto newPrice)
        {
            if (newPrice == null)
            {
                return BadRequest("Invalid electricity price data.");
            }

            newPrice.DateTime = DateTime.UtcNow;

            return CreatedAtAction(nameof(GetAveragePrices), new { date = newPrice.DateTime }, newPrice);
        }

        private List<ElectricityPriceDto> FilterEverySeven(List<ElectricityPriceDto> input)
        {
            var filtered = new List<ElectricityPriceDto>();
            for (int i = 0; i < input.Count; i += 7)
            {
                filtered.Add(input[i]);
            }
            return filtered;
        }



    }
}
