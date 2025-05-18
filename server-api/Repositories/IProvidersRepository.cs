using electricity_provider_server_api.DTOs;
using electricity_provider_server_api.Models;

namespace electricity_provider_server_api.Repositories
{
    public interface IProvidersRepository
    {
        Task<List<Provider>> GetAllProvidersAsync();
        Task<List<ProviderWithRatingAndIdDto>> GetProvidersWithRatingsAsync();
        Task<Provider> GetProviderByIdAsync(int id);
        Task<Provider> GetProviderByNameAsync(string name);
        Task<Provider> AddProviderAsync(Provider provider);
        Task UpdateProviderAsync(Provider provider);
        Task DeleteProviderAsync(int id);
    }
}
