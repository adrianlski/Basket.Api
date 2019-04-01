using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BasketApi.Dtos;
using BasketApi.Models;
using BasketApi.Repositories;

namespace BasketApi.Services
{
    public class BasketService : IBasketService
    {
        private readonly IRepository<BasketItem> _basketRepository;
        private readonly IMapper _mapper;

        public BasketService(IRepository<BasketItem> repository, IMapper mapper)
        {
            _basketRepository = repository;
            _mapper = mapper;
        }

        public async Task<BasketToReturnDto> GetBasket(int customerId)
        {
            var items = await _basketRepository.GetManyAsync(x => x.CustomerId == customerId);
            var basketToReturn = _mapper.Map<BasketToReturnDto>(items);

            return basketToReturn;
        }

        public async Task<bool> AddItemToBasket(int customerId, ItemToAddDto itemToAddDto)
        {
            if (await _basketRepository.ExistsAsync(x => x.CustomerId == customerId && x.ItemId == itemToAddDto.ItemId))
            {
                return true;
            }

            var basketToSave = _mapper.Map<ItemToAddDto, BasketItem>(itemToAddDto);
            basketToSave.CustomerId = customerId;

            _basketRepository.Add(basketToSave);

            return await _basketRepository.SaveAllAsync();
        }

        public Task<bool> UpdateBasketItem(int customerId, ItemToUpdateDto itemToUpdateDto)
        {
            throw new NotImplementedException();
        }
    }
}
