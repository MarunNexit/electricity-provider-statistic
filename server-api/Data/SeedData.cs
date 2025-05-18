using electricity_provider_server_api.Controllers;
using electricity_provider_server_api.DTOs;
using electricity_provider_server_api.Models;
using electricity_provider_server_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.Intrinsics.X86;
using Microsoft.Extensions.Logging;
using electricity_provider_server_api.DTOs.Auth;
using System.Text.Json;

namespace electricity_provider_server_api.Data
{
    public static class SeedData
    {

        public class PostcodeResponse
        {
            public int Status { get; set; }
            public PostcodeResult? Result { get; set; }
        }

        public class PostcodeResult
        {
            public string? Postcode { get; set; }
            public string? Country { get; set; }
            public string? Region { get; set; }
            public string? AdminDistrict { get; set; }
            public string? AdminWard { get; set; }
        }

        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());

            var authService = serviceProvider.GetRequiredService<AuthService>();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("SeedData");

            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                logger.LogInformation("Starting database seeding...");

                await SeedProvidersAsync(context, logger);
                await SeedUsersAsync(context, authService, logger);
                await SeedUserAddressesAsync(context, logger);
                await SeedUserProviderChoicesAsync(context, logger);
                await SeedProviderRatingsAsync(context, logger);

                // Uncomment and implement additional seed methods as needed
                // await SeedTariffsAsync(context, logger);
                // await SeedProviderAddressesAsync(context, logger);
                // await SeedUserAddressesAsync(context, logger);

                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                logger.LogInformation("Database seeding completed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during database seeding.");
                await transaction.RollbackAsync();
                throw;
            }
        }

        private static readonly HttpClient _httpClient = new();

        private static async Task SeedUserAddressesAsync(AppDbContext context, ILogger logger)
        {
            if (context.UserAddress.Any()) return;

            var userIds = await context.Users.Select(u => u.Id).ToListAsync();
            var rnd = new Random();

            var addedCount = 0;
            int maxAttempts = userIds.Count * 3;

            int attempts = 0;
            while (addedCount < userIds.Count && attempts < maxAttempts)
            {
                attempts++;
                var response = await _httpClient.GetAsync("https://api.postcodes.io/random/postcodes");
                if (!response.IsSuccessStatusCode) continue;

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<PostcodeResponse>(json, options);

                if (result?.Result == null) continue;

                var country = result.Result.Country;
                bool isEngland = country == "England";
                bool allowOtherUK = rnd.NextDouble() < 0.1;

                if (!isEngland && !allowOtherUK)
                    continue;

                var userId = userIds[addedCount];

                var address = new UserAddress
                {
                    UserId = userId,
                    City = result.Result.AdminDistrict ?? result.Result.Region ?? "Unknown City",
                    Region = result.Result.Region ?? "Unknown Region",
                    Street = $"{rnd.Next(1, 100)} {result.Result.AdminWard ?? "Main Street"}",
                    ApartmentNumber = rnd.Next(1, 30).ToString(),
                    Postcode = result.Result.Postcode,
                    Country = country,
                };

                context.UserAddress.Add(address);
                addedCount++;
            }

            await context.SaveChangesAsync();
            logger.LogInformation($"Seeded {addedCount} user addresses.");
        }

        private static async Task SeedUserProviderChoicesAsync(AppDbContext context, ILogger logger)
        {
            if (context.UserProviderChoices.Any()) return;

            var rnd = new Random();
            var userIds = await context.UserAddress.Select(u => u.Id).ToListAsync();
            var providerIds = await context.Providers.Select(p => p.Id).ToListAsync();

            foreach (var userId in userIds)
            {
                var providerId = providerIds[rnd.Next(providerIds.Count)];

                var startDate = RandomDate(new DateTime(2010, 1, 1), new DateTime(2025, 5, 17), rnd);
                DateTime? endDate = null;

                // 50% шанс, що контракт ще не завершився
                if (rnd.NextDouble() > 0.5)
                {
                    endDate = RandomDate(startDate.AddMonths(6), startDate.AddYears(5), rnd);
                }

                var choice = new UserProviderChoice
                {
                    UserAddressId = userId,
                    ProviderId = providerId,
                    ChoiceDate = startDate,
                    ContractEndDate = endDate,
                };

                context.UserProviderChoices.Add(choice);
            }

            await context.SaveChangesAsync();
            logger.LogInformation("Seeded provider choices.");
        }

        private static DateTime RandomDate(DateTime from, DateTime to, Random rnd)
        {
            var range = (to - from).Days;
            return from.AddDays(rnd.Next(range));
        }

        private static async Task SeedProvidersAsync(AppDbContext context, ILogger logger)
        {
            if (context.Providers.Any()) return;

            context.Providers.AddRange(
                new Provider
                {
                    Name = "British Gas",
                        Website = "https://www.britishgas.co.uk",
                        Logo = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR7IVDWYwAHHzj8sIQkfTAzeP3YJhH-sJ_pCw&s"
                },
                new Provider
                {
                    Name = "EDF",
                    Website = "https://www.edfenergy.com",
                    Logo = "https://cdn.worldvectorlogo.com/logos/edf-2.svg"
                },
                new Provider
                {
                    Name = "E.ON",
                    Website = "https://www.eonenergy.com",
                    Logo = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTd5eppWIfHi7bi3fle9xQDneXo0uNOF6WyBA&s"
                },
                new Provider
                {
                    Name = "npower",
                    Website = "https://npowerbusinesssolutions.com",
                    Logo = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/91/Logo_npower.svg/1200px-Logo_npower.svg.png"
                },
                new Provider
                {
                    Name = "Scottish Power",
                    Website = "https://www.scottishpower.co.uk",
                    Logo = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRT5x8uHEeNoQU-dTX-drzeothWsesQVde8xw&s"
                },
                new Provider
                {
                    Name = "SSE",
                    Website = "https://www.sseenergysolutions.co.uk",
                    Logo = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSYnMon_R8KXfh_bafSB_ePensqFApg3u7EAw&s"
                },
                new Provider
                {
                    Name = "Shell Energy",
                    Website = "https://uk.shellenergy.com",
                    Logo = "https://149516224.v2.pressablecdn.com/wp-content/uploads/2021/08/shellLogo_sm_537.jpg"
                },
                new Provider
                {
                    Name = "OVO",
                    Website = "https://www.ovoenergy.com",
                    Logo = "https://media.product.which.co.uk/prod/images/ar_2to1_1500x750/5617b1d36737-ovo-logoglide.jpg"
                },
                new Provider
                {
                    Name = "Utilita",
                    Website = "https://utilita.co.uk",
                    Logo = "https://media.product.which.co.uk/prod/images/original/848ceddcad9a-utilita.jpg"
                },
                new Provider
                {
                    Name = "Utility Warehouse",
                    Website = "https://uw.co.uk",
                    Logo = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTXnouTU85LceABXtuguL7VBTeDQuLJSKskiA&s"
                },
                new Provider
                {
                    Name = "Bulb Energy",
                    Website = "https://accountancycloud.com/blogs/bulb-energy-the-company-that-fell-victim-to-the-energy-crisis",
                    Logo = "https://www.lee.me.uk/wp-content/uploads/2019/09/bulb-logo.jpg"
                },
                new Provider
                {
                    Name = "Octopus Energy",
                    Website = "https://octopus.energy",
                    Logo = "https://play-lh.googleusercontent.com/uJaTdao2WJMQZ97simDqwMzwFeXP9T0xdmAIcub15vmPF753Q4xoC5QiXJv0DuNJT6-S"
                },
                new Provider
                {
                    Name = "Avro Energy",
                    Website = "https://www.businessenergyuk.com",
                    Logo = "https://www.switchcraft.co.uk/wp-content/uploads/2019/11/avro.png"
                },
                new Provider
                {
                    Name = "Green Network Energy",
                    Website = "https://genational.co.uk",
                    Logo = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTTLZXr1tD5NvcJcfbxIBOs4yikjCJuZW42eA&s"
                },
                new Provider
                {
                    Name = "So Energy",
                    Website = "https://www.so.energy",
                    Logo = "https://media.product.which.co.uk/prod/images/original/221b191375ad-so-energy-logo.jpg"
                }
            );

            await context.SaveChangesAsync();
        }

        private static async Task SeedProviderAddressesAsync(AppDbContext context, ILogger logger)
        {
            if (context.ProviderAddress.Any()) return;

            context.ProviderAddress.AddRange(
                new ProviderAddress
                {
                    ProviderId = 1,
                    Street = "Main Street 1",
                    City = "Kyiv",
                    Country = "England"
                },
                new ProviderAddress
                {
                    ProviderId = 2,
                    Street = "Green Avenue 5",
                    City = "Lviv",
                    Country = "England"
                }
            );

            await context.SaveChangesAsync();
        }

        private static async Task SeedUsersAsync(AppDbContext context, AuthService authService, ILogger logger)
        {
            if (context.Users.Any()) return;

            for (int i = 1; i <= 1000; i++)
            {
                string email = $"test_{i}@example.com";

                var existingUser = await context.Users.AnyAsync(u => u.Email == email);
                if (existingUser)
                    continue;

                var registerDto = new RegisterDto
                {
                    Email = email,
                    Password = "123"
                };

                await authService.Register(registerDto);
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedTariffsAsync(AppDbContext context, ILogger logger)
        {
            if (context.Tariffs.Any()) return;

            context.Tariffs.AddRange(
                new Tariff
                {
                    ProviderId = 1,
                    Name = "Basic Tariff",
                    Price = 0.25m,
                    IsAvailable = true
                },
                new Tariff
                {
                    ProviderId = 2,
                    Name = "Eco Tariff",
                    Price = 0.20m,
                    IsAvailable = true
                }
            );

            await context.SaveChangesAsync();
        }

        private static async Task SeedProviderRatingsAsync(AppDbContext context, ILogger logger)
        {
            if (context.ProviderRatings.Any()) return;

            var choices = await context.UserProviderChoices
                .Include(c => c.UserAddress)
                .ToListAsync();

            var random = new Random();
            var ratings = new List<ProviderRating>();

            foreach (var choice in choices)
            {
                var userId = choice.UserAddress.UserId;
                var providerId = choice.ProviderId;

                if (ratings.Any(r => r.UserId == userId && r.ProviderId == providerId))
                    continue;

                var rating = Math.Round(random.NextDouble() * 4.5 + 0.5, 1);
                var date = RandomDateRatings(new DateTime(2020, 1, 1), DateTime.UtcNow, random);

                ratings.Add(new ProviderRating
                {
                    UserId = userId,
                    ProviderId = providerId,
                    Rating = rating,
                    IsActive = true,
                    Date = date
                });
            }

            await context.ProviderRatings.AddRangeAsync(ratings);
            await context.SaveChangesAsync();

            logger.LogInformation("✅ ProviderRatings seeded successfully.");
        }

        private static DateTime RandomDateRatings(DateTime from, DateTime to, Random rand)
        {
            var range = (to - from).Days;
            return from.AddDays(rand.Next(range)).AddHours(rand.Next(0, 24)).AddMinutes(rand.Next(0, 60));
        }




    }
}
