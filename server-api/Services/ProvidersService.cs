using AutoMapper;
using electricity_provider_server_api.DTOs;
using electricity_provider_server_api.Models;
using electricity_provider_server_api.Repositories;

namespace electricity_provider_server_api.Services
{
    public class ProvidersService
    {
        private readonly IProvidersRepository _repository;
        private readonly IMapper _mapper;

        public ProvidersService(IProvidersRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ProviderWithIdDto>> GetAllProvidersAsync()
        {
            var providers = await _repository.GetAllProvidersAsync();
            return _mapper.Map<List<ProviderWithIdDto>>(providers);
        }

        public async Task<List<ProviderWithRatingAndIdDto>> GetProvidersWithRatingsAsync()
        {
            return await _repository.GetProvidersWithRatingsAsync();
        }

        public async Task<ProviderWithRatingAndIdDto> GetProviderByIdWithRatingsAsync(int id)
        {
            var provider = await _repository.GetProviderByIdAsync(id);
            return provider == null ? null : _mapper.Map<ProviderWithRatingAndIdDto>(provider);
        }

        public async Task<ProviderWithIdDto> GetProviderByIdAsync(int id)
        {
            var provider = await _repository.GetProviderByIdAsync(id);
            return provider == null ? null : _mapper.Map<ProviderWithIdDto>(provider);
        }

        public async Task<ProviderWithIdDto> GetProviderByNameAsync(string name)
        {
            var provider = await _repository.GetProviderByNameAsync(name);
            return provider == null ? null : _mapper.Map<ProviderWithIdDto>(provider);
        }

        public async Task<Provider> AddProviderAsync(ProviderDto providerDto)
        {
            var provider = _mapper.Map<Provider>(providerDto);
            return await _repository.AddProviderAsync(provider);
        }

        public async Task UpdateProviderAsync(int id, ProviderDto updatedProviderDto)
        {
            var provider = _mapper.Map<Provider>(updatedProviderDto);
            provider.Id = id;

            await _repository.UpdateProviderAsync(provider);
        }

        public async Task DeleteProviderAsync(int id)
        {
            await _repository.DeleteProviderAsync(id);
        }
    }

}
