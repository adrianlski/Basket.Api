using System.Collections.Generic;
using AutoMapper;
using BasketApi.Dtos;
using BasketApi.Models;

namespace BasketApi.Mappings.Resolvers
{
    public class BasketToReturnDtoResolver : IValueResolver<List<BasketItem>, BasketToReturnDto, List<ItemToReturnDto>>
    {
        public List<ItemToReturnDto> Resolve(List<BasketItem> source, BasketToReturnDto destination, List<ItemToReturnDto> destMember, ResolutionContext context)
        {
            var items = new List<ItemToReturnDto>();

            foreach (var item in source)
            {
                items.Add(new ItemToReturnDto
                {
                    ItemId = item.Item.Id,
                    ItemName = item.Item.Name,
                    Quantity = item.Quantity
                });
            }

            return items;
        }
    }
}
