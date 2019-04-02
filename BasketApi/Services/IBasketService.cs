using BasketApi.Dtos;
using System.Threading.Tasks;

namespace BasketApi.Services
{
    public interface IBasketService
    {
        Task<BasketToReturnDto> GetBasket(int customerId);
        Task<bool> AddItemToBasket(int customerId, ItemToAddDto itemToAddDto);
        Task<bool> RemoveItemFromBasktet(int customerId, int itemId);
        Task<bool> UpdateBasketItem(int customerId, ItemToUpdateDto itemToUpdateDto);
        Task<bool> ClearBasket(int customerId);
    }
}
