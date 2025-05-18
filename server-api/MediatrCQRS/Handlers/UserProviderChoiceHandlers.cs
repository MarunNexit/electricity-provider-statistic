using AutoMapper;
using electricity_provider_server_api.Data;
using electricity_provider_server_api.DTOs;
using electricity_provider_server_api.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static electricity_provider_server_api.MediatrCQRS.QueriesAndCommands.ChoicesQueriesAndCommands;

namespace electricity_provider_server_api.MediatrCQRS.Handlers
{
    public class UserProviderChoiceHandlers :
    IRequestHandler<GetAllUserProviderChoicesQuery, IEnumerable<GetUserProviderChoiceDtoWithId>>,
    IRequestHandler<GetUserProviderChoiceByIdQuery, UserProviderChoiceDtoWithId?>,
    IRequestHandler<GetUserProviderChoicesByUserIdQuery, IEnumerable<UserProviderChoiceDtoWithId>>,
    IRequestHandler<GetUserProviderChoicesByProviderIdQuery, IEnumerable<UserProviderChoiceDtoWithId>>,
    IRequestHandler<GetUserCountByProviderQuery, int>,
    IRequestHandler<CreateUserProviderChoiceCommand, UserProviderChoiceDtoWithId>,
    IRequestHandler<UpdateUserProviderChoiceCommand, bool>,
    IRequestHandler<DeleteUserProviderChoiceCommand, bool>,
    IRequestHandler<GetUserCountsForAllProvidersQuery, IEnumerable<ProviderUserCountDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserProviderChoiceHandlers(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetUserProviderChoiceDtoWithId>> Handle(GetAllUserProviderChoicesQuery request, CancellationToken cancellationToken)
        {
            var entities = await _context.UserProviderChoices.Include(x => x.UserAddress).Include(x => x.Provider).Include(x => x.Tariff).ToListAsync();
            return _mapper.Map<IEnumerable<GetUserProviderChoiceDtoWithId>>(entities);
        }

        public async Task<UserProviderChoiceDtoWithId?> Handle(GetUserProviderChoiceByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.UserProviderChoices.Include(x => x.UserAddress).Include(x => x.Provider).Include(x => x.Tariff)
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            return entity == null ? null : _mapper.Map<UserProviderChoiceDtoWithId>(entity);
        }

        public async Task<IEnumerable<UserProviderChoiceDtoWithId>> Handle(GetUserProviderChoicesByUserIdQuery request, CancellationToken cancellationToken)
        {
            var entities = await _context.UserProviderChoices.Where(x => x.UserAddressId == request.UserAddressId).ToListAsync();
            return _mapper.Map<IEnumerable<UserProviderChoiceDtoWithId>>(entities);
        }

        public async Task<IEnumerable<UserProviderChoiceDtoWithId>> Handle(GetUserProviderChoicesByProviderIdQuery request, CancellationToken cancellationToken)
        {
            var entities = await _context.UserProviderChoices.Where(x => x.ProviderId == request.ProviderId).ToListAsync();
            return _mapper.Map<IEnumerable<UserProviderChoiceDtoWithId>>(entities);
        }

        public async Task<int> Handle(GetUserCountByProviderQuery request, CancellationToken cancellationToken)
        {
            return await _context.UserProviderChoices.Where(x => x.ProviderId == request.ProviderId).Select(x => x.UserAddressId).Distinct().CountAsync();
        }

        public async Task<IEnumerable<ProviderUserCountDto>> Handle(GetUserCountsForAllProvidersQuery request, CancellationToken cancellationToken)
        {
            var choices = await _context.UserProviderChoices
                .Include(c => c.Provider)
                .Include(c => c.UserAddress)
                .ThenInclude(a => a.User)
                .ToListAsync();

            var grouped = choices
                .GroupBy(c => c.Provider.Name)
                .Select(g => new ProviderUserCountDto
                {
                    Id = g.First().Provider.Id,
                    ProviderName = g.Key,
                    UserCount = g.Select(x => x.UserAddress.UserId).Distinct().Count()
                })
                .OrderByDescending(x => x.UserCount)
                .ToList();

            return grouped;
        }

        public async Task<UserProviderChoiceDtoWithId> Handle(CreateUserProviderChoiceCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var user = await _context.UserAddress.FindAsync(dto.UserAddressId);
            var provider = await _context.Providers.FindAsync(dto.ProviderId);
            var tariff = dto.TariffId.HasValue ? await _context.Tariffs.FindAsync(dto.TariffId) : null;

            if (user == null || provider == null || (dto.TariffId.HasValue && tariff == null))
                throw new ArgumentException("User, Provider, or Tariff not found");

            var entity = _mapper.Map<UserProviderChoice>(dto);
            _context.UserProviderChoices.Add(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserProviderChoiceDtoWithId>(entity);
        }

        public async Task<bool> Handle(UpdateUserProviderChoiceCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.UserProviderChoices.FindAsync(request.Id);
            if (entity == null) return false;

            var dto = request.Dto;
            entity.UserAddressId = dto.UserAddressId;
            entity.ProviderId = dto.ProviderId;
            entity.ChoiceDate = dto.ChoiceDate;
            entity.ContractEndDate = dto.ContractEndDate;
            entity.TariffId = dto.TariffId == 0 ? null : dto.TariffId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Handle(DeleteUserProviderChoiceCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.UserProviderChoices.FindAsync(request.Id);
            if (entity == null) return false;

            _context.UserProviderChoices.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
