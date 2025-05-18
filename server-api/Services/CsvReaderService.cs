using electricity_provider_server_api.DTOs;
using System.Globalization;
using System.Text;

namespace electricity_provider_server_api.Services
{
    public static class CsvReaderService
    {
        public static List<ElectricityMarketShareDto> ReadElectricityMarketShares(string relativePath)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);

            var result = new List<ElectricityMarketShareDto>();
            var lines = File.ReadAllLines(filePath);
            if (lines.Length == 0) return result;

            var headers = lines[1].Split(',').Skip(1).ToList();

            foreach (var line in lines.Skip(2))
            {
                var values = line.Split(',');
                if (values.Length < 1) continue;

                var record = new ElectricityMarketShareDto
                {
                    Quarter = values[0].Trim('"'),
                    SupplierShares = new Dictionary<string, double?>()
                };

                Console.WriteLine($"Quarter: {headers}");

                for (int i = 1; i < values.Length; i++)
                {
                    var headerKey = headers[i - 1];
                    var value = values[i];

                    if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out double share))
                    {
                        record.SupplierShares[headerKey] = share;
                        Console.WriteLine($"  {headerKey} -> {share}");
                    }
                    else
                    {
                        record.SupplierShares[headerKey] = null;
                        Console.WriteLine($"  {headerKey} -> null");
                    }
                }

                result.Add(record);
            }

            return result;
        }


        public static async Task<List<ElectricityPriceDto>> LoadElectricityPricesAsync(string relativePath)
        {
            var prices = new List<ElectricityPriceDto>();
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);

            using var reader = new StreamReader(filePath, Encoding.UTF8);
            string? line;
            bool isFirstLine = true;

            var format = "yyyy-MM-dd HH:mm:ss";
            var provider = CultureInfo.InvariantCulture;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (isFirstLine)
                {
                    isFirstLine = false;
                    continue;
                }

                var parts = line.Split(',');
                if (parts.Length != 2)
                    continue;

                if (DateTime.TryParseExact(parts[0].Trim('"'), format, provider, DateTimeStyles.None, out var date) &&
                    decimal.TryParse(parts[1], NumberStyles.Any, provider, out var price))
                {
                    prices.Add(new ElectricityPriceDto
                    {
                        DateTime = date,
                        Price = price
                    });
                }
            }

            return prices;
        }

        public static List<MarketShareGroupDto> GetMarketShareByGroup(List<ElectricityMarketShareDto> data, int year = 2024)
        {
            var bigSix = new HashSet<string> { "British Gas", "EDF", "E.ON", "npower", "Scottish Power", "SSE" };
            var newProviders = new HashSet<string> { "Octopus", "OVO", "Shell", "Bulb" };

            var bigSixTotal = 0.0;
            var newProvidersTotal = 0.0;
            var smallSuppliersTotal = 0.0;
            var validQuarterCount = 0;

            foreach (var item in data)
            {
                if (!item.Quarter.StartsWith($"{year}")) continue;

                var totalForQuarter = item.SupplierShares.Values.Where(v => v.HasValue).Sum(v => v.Value);

                if (totalForQuarter == 0) continue;

                double bigSixSum = 0, newSum = 0, smallSum = 0;

                foreach (var kv in item.SupplierShares)
                {
                    var name = kv.Key.Trim('"');
                    if (!kv.Value.HasValue) continue;

                    if (bigSix.Contains(name))
                        bigSixSum += kv.Value.Value;
                    else if (newProviders.Contains(name))
                        newSum += kv.Value.Value;
                    else
                        smallSum += kv.Value.Value;
                }

                bigSixTotal += bigSixSum / totalForQuarter * 100;
                newProvidersTotal += newSum / totalForQuarter * 100;
                smallSuppliersTotal += smallSum / totalForQuarter * 100;
                validQuarterCount++;
            }

            if (validQuarterCount == 0)
            {
                return new List<MarketShareGroupDto>
                {
                    new() { GroupName = "Big Six", Share = 0 },
                    new() { GroupName = "New Providers", Share = 0 },
                    new() { GroupName = "Small Suppliers", Share = 0 }
                };
            }

            return new List<MarketShareGroupDto>
            {
                new() { GroupName = "Big Six", Share = Math.Round(bigSixTotal / validQuarterCount, 2) },
                new() { GroupName = "New Providers", Share = Math.Round(newProvidersTotal / validQuarterCount, 2) },
                new() { GroupName = "Small Suppliers", Share = Math.Round(smallSuppliersTotal / validQuarterCount, 2) }
            };
        }

        public static List<MarketShareGroupDetailedDto> GetMarketShareByGroupDetailed(List<ElectricityMarketShareDto> data, int year = 2024)
        {
            var bigSix = new HashSet<string> { "British Gas", "EDF", "E.ON", "npower", "Scottish Power", "SSE" };
            var newProviders = new HashSet<string> { "Octopus Energy", "OVO", "Shell Energy", "Bulb Energy" };

            double bigSixTotal = 0, newProvidersTotal = 0, smallSuppliersTotal = 0;
            int validQuarterCount = 0;

            var bigSixProviders = new Dictionary<string, double>();
            var newProvidersDict = new Dictionary<string, double>();
            var smallSuppliersDict = new Dictionary<string, double>();

            foreach (var item in data)
            {
                if (!item.Quarter.StartsWith($"{year}")) continue;

                var totalForQuarter = item.SupplierShares.Values.Where(v => v.HasValue).Sum(v => v.Value);
                if (totalForQuarter == 0) continue;

                double bigSixSum = 0, newSum = 0, smallSum = 0;

                foreach (var kv in item.SupplierShares)
                {
                    var name = kv.Key.Trim('"');
                    if (!kv.Value.HasValue) continue;

                    double sharePercent = kv.Value.Value / totalForQuarter * 100;

                    if (bigSix.Contains(name))
                    {
                        bigSixSum += sharePercent;
                        if (!bigSixProviders.ContainsKey(name))
                            bigSixProviders[name] = 0;
                        bigSixProviders[name] += sharePercent;
                    }
                    else if (newProviders.Contains(name))
                    {
                        newSum += sharePercent;
                        if (!newProvidersDict.ContainsKey(name))
                            newProvidersDict[name] = 0;
                        newProvidersDict[name] += sharePercent;
                    }
                    else
                    {
                        smallSum += sharePercent;
                        if (!smallSuppliersDict.ContainsKey(name))
                            smallSuppliersDict[name] = 0;
                        smallSuppliersDict[name] += sharePercent;
                    }
                }

                bigSixTotal += bigSixSum;
                newProvidersTotal += newSum;
                smallSuppliersTotal += smallSum;
                validQuarterCount++;
            }

            if (validQuarterCount == 0)
            {
                return new List<MarketShareGroupDetailedDto>
                {
                    new MarketShareGroupDetailedDto
                    {
                        GroupName = "Big Six",
                        Share = 0,
                        Providers = new List<ProviderShareDto>()
                    },
                    new MarketShareGroupDetailedDto
                    {
                        GroupName = "New Providers",
                        Share = 0,
                        Providers = new List<ProviderShareDto>()
                    },
                    new MarketShareGroupDetailedDto
                    {
                        GroupName = "Small Suppliers",
                        Share = 0,
                        Providers = new List<ProviderShareDto>()
                    },
                };
            }

            List<ProviderShareDto> MakeProviderList(Dictionary<string, double> dict)
            {
                return dict.Select(kv => new ProviderShareDto
                {
                    Name = kv.Key,
                    Share = Math.Round(kv.Value / validQuarterCount, 2)
                })
                .OrderByDescending(p => p.Share)
                .ToList();
            }

            return new List<MarketShareGroupDetailedDto>
            {
                new MarketShareGroupDetailedDto
                {
                    GroupName = "Big Six",
                    Share = Math.Round(bigSixTotal / validQuarterCount, 2),
                    Providers = MakeProviderList(bigSixProviders)
                },
                new MarketShareGroupDetailedDto
                {
                    GroupName = "New Providers",
                    Share = Math.Round(newProvidersTotal / validQuarterCount, 2),
                    Providers = MakeProviderList(newProvidersDict)
                },
                new MarketShareGroupDetailedDto
                {
                    GroupName = "Small Suppliers",
                    Share = Math.Round(smallSuppliersTotal / validQuarterCount, 2),
                    Providers = MakeProviderList(smallSuppliersDict)
                },
            };
        }
    }

}
