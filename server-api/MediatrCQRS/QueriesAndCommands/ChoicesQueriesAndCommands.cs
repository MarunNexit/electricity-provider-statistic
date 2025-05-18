using electricity_provider_server_api.DTOs;
using MediatR;

namespace electricity_provider_server_api.MediatrCQRS.QueriesAndCommands
{
    public class ChoicesQueriesAndCommands
    {
        // Queries
        public record GetAllUserProviderChoicesQuery() : IRequest<IEnumerable<GetUserProviderChoiceDtoWithId>>;
        public record GetUserProviderChoiceByIdQuery(int Id) : IRequest<UserProviderChoiceDtoWithId?>;
        public record GetUserProviderChoicesByUserIdQuery(int UserAddressId) : IRequest<IEnumerable<UserProviderChoiceDtoWithId>>;
        public record GetUserProviderChoicesByProviderIdQuery(int ProviderId) : IRequest<IEnumerable<UserProviderChoiceDtoWithId>>;
        public record GetUserCountByProviderQuery(int ProviderId) : IRequest<int>;
        public record GetUserCountsForAllProvidersQuery() : IRequest<IEnumerable<ProviderUserCountDto>>;

        // Commands
        public record CreateUserProviderChoiceCommand(UserProviderChoiceDto Dto) : IRequest<UserProviderChoiceDtoWithId>;
        public record UpdateUserProviderChoiceCommand(int Id, UserProviderChoiceDto Dto) : IRequest<bool>;
        public record DeleteUserProviderChoiceCommand(int Id) : IRequest<bool>;

    }
}
