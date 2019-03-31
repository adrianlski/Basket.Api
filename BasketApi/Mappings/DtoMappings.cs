using AutoMapper;
using BasketApi.Dtos;
using BasketApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketApi.Mappings
{
    public class DtoMappings : Profile
    {
        public DtoMappings()
        {
            CreateMap<List<BasketItem>, BasketToReturnDto>()
                .ForMember(dest => dest.CustomerId, opts => opts.MapFrom(src => src.FirstOrDefault().CustomerId));
        }
    }
}
