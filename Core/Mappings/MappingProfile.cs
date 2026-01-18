using AutoMapper;
using SmartStoreReservation.Core.DTOs;
using SmartStoreReservation.Core.Entities;

namespace SmartStoreReservation.Core.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Product mappings
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.ShopName, opt => opt.MapFrom(src => src.Shop!.Name))
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => 
                src.ProductCategories.Select(pc => pc.Category!.Name).ToList()));

        CreateMap<CreateProductDto, Product>()
            .ForMember(dest => dest.ProductCategories, opt => opt.Ignore());

        CreateMap<UpdateProductDto, Product>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // Reservation mappings
        CreateMap<Reservation, ReservationDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User!.Name))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product!.Name))
            .ForMember(dest => dest.CabinNumber, opt => opt.MapFrom(src => src.Cabin!.CabinNumber));

        CreateMap<CreateReservationDto, Reservation>()
            .ForMember(dest => dest.Hour, opt => opt.MapFrom(src => TimeSpan.Parse(src.Hour)));

        // Cabin mappings
        CreateMap<Cabin, AvailableCabinDto>()
            .ForMember(dest => dest.ShopName, opt => opt.MapFrom(src => src.Shop!.Name))
            .ForMember(dest => dest.IsAvailable, opt => opt.Ignore());

        // User mappings
        CreateMap<User, UserDto>();
        CreateMap<RegisterDto, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
    }
}