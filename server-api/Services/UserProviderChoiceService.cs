using electricity_provider_server_api.Data;
using electricity_provider_server_api.DTOs;
using electricity_provider_server_api.Models;
using Microsoft.EntityFrameworkCore;

namespace electricity_provider_server_api.Services
{
    public interface IUserProviderChoiceService
    {
        Task<IEnumerable<UserProviderChoiceDtoWithId>> GetAllAsync();
        Task<UserProviderChoiceDtoWithId?> GetByIdAsync(int id);
        Task<UserProviderChoiceDtoWithId> CreateAsync(UserProviderChoiceDto dto);
        Task<bool> UpdateAsync(int id, UserProviderChoiceDto dto);
        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<UserProviderChoiceDtoWithId>> GetByUserIdAsync(int userId);
        Task<IEnumerable<UserProviderChoiceDtoWithId>> GetByProviderIdAsync(int providerId);
        Task<int> GetUserCountByProviderAsync(int providerId);
    }

    public class UserProviderChoiceService : IUserProviderChoiceService
    {
        private readonly AppDbContext _context;

        public UserProviderChoiceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserProviderChoiceDtoWithId>> GetAllAsync()
        {
            return await _context.UserProviderChoices
                .Select(x => MapToDtoWithId(x))
                .ToListAsync();
        }

        public async Task<UserProviderChoiceDtoWithId> GetByIdAsync(int id)
        {
            var entity = await _context.UserProviderChoices.FindAsync(id);
            return entity == null ? null : MapToDtoWithId(entity);
        }

        public async Task<UserProviderChoiceDtoWithId> CreateAsync(UserProviderChoiceDto dto)
        {
            // Ensure referenced entities exist
            if (!await _context.Users.AnyAsync(u => u.Id == dto.UserAddressId))
                throw new ArgumentException("User not found.");
            if (!await _context.Providers.AnyAsync(p => p.Id == dto.ProviderId))
                throw new ArgumentException("Provider not found.");
            if (dto.TariffId.HasValue && !await _context.Tariffs.AnyAsync(t => t.Id == dto.TariffId.Value))
                throw new ArgumentException("Tariff not found.");

            var entity = new UserProviderChoice
            {
                UserAddressId = dto.UserAddressId,
                ProviderId = dto.ProviderId,
                TariffId = dto.TariffId,
                ChoiceDate = dto.ChoiceDate,
                ContractEndDate = dto.ContractEndDate
            };

            _context.UserProviderChoices.Add(entity);
            await _context.SaveChangesAsync();

            return MapToDtoWithId(entity);
        }

        public async Task<bool> UpdateAsync(int id, UserProviderChoiceDto dto)
        {
            var entity = await _context.UserProviderChoices.FindAsync(id);
            if (entity == null) return false;

            entity.ProviderId = dto.ProviderId;
            entity.UserAddressId = dto.UserAddressId;
            entity.TariffId = dto.TariffId;
            entity.ChoiceDate = dto.ChoiceDate;
            entity.ContractEndDate = dto.ContractEndDate;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.UserProviderChoices.FindAsync(id);
            if (entity == null) return false;

            _context.UserProviderChoices.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        // 🔸 Отримати всі записи по UserId
        public async Task<IEnumerable<UserProviderChoiceDtoWithId>> GetByUserIdAsync(int userAddressId)
        {
            return await _context.UserProviderChoices
                .Where(x => x.UserAddressId == userAddressId)
                .Select(x => MapToDtoWithId(x))
                .ToListAsync();
        }

        // 🔸 Отримати всі записи по ProviderId
        public async Task<IEnumerable<UserProviderChoiceDtoWithId>> GetByProviderIdAsync(int providerId)
        {
            return await _context.UserProviderChoices
                .Where(x => x.ProviderId == providerId)
                .Select(x => MapToDtoWithId(x))
                .ToListAsync();
        }

        // 🔸 Порахувати кількість унікальних користувачів для провайдера
        public async Task<int> GetUserCountByProviderAsync(int providerId)
        {
            return await _context.UserProviderChoices
                .Where(x => x.ProviderId == providerId)
                .Select(x => x.UserAddressId)
                .Distinct()
                .CountAsync();
        }

        private static UserProviderChoiceDtoWithId MapToDtoWithId(UserProviderChoice choice)
        {
            return new UserProviderChoiceDtoWithId
            {
                Id = choice.Id,
                UserAddressId = choice.UserAddressId,
                ProviderId = choice.ProviderId,
                TariffId = choice.TariffId,
                ChoiceDate = choice.ChoiceDate,
                ContractEndDate = choice.ContractEndDate
            };
        }
    }
}
