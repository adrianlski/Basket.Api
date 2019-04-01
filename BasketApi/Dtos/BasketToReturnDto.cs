using System.Collections.Generic;

namespace BasketApi.Dtos
{
    public class BasketToReturnDto
    {
        public int CustomerId { get; set; }
        public List<ItemToReturnDto> Items { get; set; }

        public BasketToReturnDto()
        {
            Items = new List<ItemToReturnDto>();
        }
    }
}
