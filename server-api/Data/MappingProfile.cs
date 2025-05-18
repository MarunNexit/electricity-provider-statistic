using AutoMapper;
using electricity_provider_server_api.DTOs;
using electricity_provider_server_api.DTOs.Auth;
using electricity_provider_server_api.Models;
using System.Net;

namespace electricity_provider_server_api.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Provider, ProviderDto>()
                .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.Addresses));

            CreateMap<ProviderDto, Provider>()
                .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.Addresses));


            CreateMap<Provider, ProviderWithIdDto>()
                .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.Addresses));

            CreateMap<ProviderWithIdDto, Provider>()
                .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.Addresses));


            CreateMap<Provider, ProviderWithRatingDto>()
                .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.Addresses))
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src =>
                    src.ProviderRatings
                        .Where(r => r.IsActive)
                        .Average(r => (double?)r.Rating) ?? 0
                ));

            CreateMap<Provider, ProviderWithRatingAndIdDto>()
                .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.Addresses))
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src =>
                    src.ProviderRatings
                        .Where(r => r.IsActive)
                        .Average(r => (double?)r.Rating) ?? 0
                ));


            CreateMap<ProviderAddressDto, ProviderAddress>()
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country ?? "England"));

            CreateMap<ProviderAddress, ProviderAddressDto>();

            CreateMap<ProviderAddress, ProviderAddressWithIdDto>();
            CreateMap<ProviderAddressWithIdDto, ProviderAddress>()
                 .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country ?? "England"));

            CreateMap<ProviderRatingDto, ProviderRating>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true));



            CreateMap<UserProviderChoice, UserProviderChoiceDtoWithId>().ReverseMap();
            CreateMap<UserProviderChoice, UserProviderChoiceDto>().ReverseMap();

            CreateMap<UserProviderChoice, GetUserProviderChoiceDtoWithId>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.UserAddress))
                .ForMember(dest => dest.Provider, opt => opt.MapFrom(src => src.Provider));
            CreateMap<UserAddress, ChoiceUserAddressDto>();
            CreateMap<Provider, ChoiceProviderDto>();

            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        }
    }
}
