using AutoMapper;
using BasketApi.Dtos;
using BasketApi.Models;
using System.Collections.Generic;
using System.Linq;
using BasketApi.Mappings.Resolvers;

namespace BasketApi.Mappings
{
    public class DtoMappings : Profile
    {
        public DtoMappings()
        {
            CreateMap<List<BasketItem>, BasketToReturnDto>()
                .ForMember(dest => dest.CustomerId, opts => opts.MapFrom(src => src.FirstOrDefault().CustomerId))
                .ForMember(dest => dest.Items, opts => opts.MapFrom<BasketToReturnDtoResolver>());

            CreateMap<ItemToAddDto, BasketItem>();
            CreateMap<ItemToUpdateDto, BasketItem>();
        }
    }
}
