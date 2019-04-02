using System.ComponentModel.DataAnnotations;

namespace BasketApi.Dtos
{
    public class ItemToUpdateDto
    {
        public int ItemId { get; set; }
        [MinLength(1)]
        public int Quantity { get; set; }
    }
}
