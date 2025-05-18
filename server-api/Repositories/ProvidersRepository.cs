using AutoMapper;
using AutoMapper.QueryableExtensions;
using electricity_provider_server_api.Data;
using electricity_provider_server_api.DTOs;
using electricity_provider_server_api.Models;
using Microsoft.EntityFrameworkCore;

namespace electricity_provider_server_api.Repositories
{
    public class ProvidersRepository : IProvidersRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProvidersRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Provider>> GetAllProvidersAsync()
        {
            return await _context.Providers.Include(p => p.Addresses).ToListAsync();
        }

        public async Task<List<ProviderWithRatingAndIdDto>> GetProvidersWithRatingsAsync()
        {
            return await _context.Providers
                .Include(p => p.Addresses)
                .ProjectTo<ProviderWithRatingAndIdDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<Provider> GetProviderByIdAsync(int id)
        {
            return await _context.Providers
                .Include(p => p.Addresses)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Provider> GetProviderByNameAsync(string name)
        {
            return await _context.Providers
                .Include(p => p.Addresses)
                .FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());
        }

        public async Task<Provider> AddProviderAsync(Provider provider)
        {
            _context.Providers.Add(provider);
            await _context.SaveChangesAsync();

            return await _context.Providers
                .Include(p => p.Addresses)
                .FirstOrDefaultAsync(p => p.Id == provider.Id);
        }

        public async Task UpdateProviderAsync(Provider provider)
        {
            _context.Providers.Update(provider);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProviderAsync(int id)
        {
            var provider = await GetProviderByIdAsync(id);
            if (provider != null)
            {
                _context.Providers.Remove(provider);
                await _context.SaveChangesAsync();
            }
        }
    }

}
