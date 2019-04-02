using System.ComponentModel.DataAnnotations;

namespace BasketApi.Dtos
{
    public class ItemToAddDto
    {
        public int ItemId { get; set; }
        [MinLength(1)]
        public int Quantity { get; set; }
    }
}
