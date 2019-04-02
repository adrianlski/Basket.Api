using System.ComponentModel.DataAnnotations;

namespace BasketApi.Dtos
{
    public class ItemToAddDto
    {
        public int ItemId { get; set; }
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
