using System.Threading.Tasks;
using AutoMapper;
using BasketApi.Dtos;
using BasketApi.Exceptions;
using BasketApi.Models;
using BasketApi.Repositories;
using Microsoft.EntityFrameworkCore.Internal;

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
                throw new ItemExistsInTheBasketException();
            }

            var basketToSave = _mapper.Map<ItemToAddDto, BasketItem>(itemToAddDto);
            basketToSave.CustomerId = customerId;

            _basketRepository.Add(basketToSave);

            return await _basketRepository.SaveAllAsync();
        }

        public async Task<bool> RemoveItemFromBasktet(int customerId, int itemId)
        {
            var itemToRemove = await _basketRepository.GetOneAsync(x => x.CustomerId == customerId && x.ItemId == itemId);

            if (itemToRemove == null)
            {
                throw new ItemNotInTheBasketException();
            }

            _basketRepository.DeleteOne(itemToRemove);

            return await _basketRepository.SaveAllAsync();
        }

        public async Task<bool> UpdateBasketItem(int customerId, ItemToUpdateDto itemToUpdateDto)
        {
            var itemFromRepo = await _basketRepository.GetOneAsync(x => x.CustomerId == customerId && x.ItemId == itemToUpdateDto.ItemId);

            if (itemFromRepo == null)
            {
                throw new ItemNotInTheBasketException();
            }

            var item = _mapper.Map(itemToUpdateDto, itemFromRepo);
            item.CustomerId = customerId;

            return await _basketRepository.SaveAllAsync();
        }

        public async Task<bool> ClearBasket(int customerId)
        {
            var items = await _basketRepository.GetManyAsync(x => x.CustomerId == customerId);

            if (!items.Any())
            {
                return true;
            }
            
            _basketRepository.DeleteRange(items);

            return await _basketRepository.SaveAllAsync();
        }
    }
}
