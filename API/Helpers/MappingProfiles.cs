using API.DTOs;
using AutoMapper;
using Domain.Model.Entities;
using Domain.Model.Entities.Identity;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dto => dto.ProductBrand, x => x.MapFrom(p => p.ProductBrand.Name))
                .ForMember(dto => dto.ProductType, x => x.MapFrom(p => p.ProductType.Name))
                .ForMember(dto => dto.PictureUrl, x => x.MapFrom<ProductUrlResolver>());

            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
