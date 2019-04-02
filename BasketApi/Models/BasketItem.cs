using System.ComponentModel.DataAnnotations.Schema;

namespace BasketApi.Models
{
    public class BasketItem
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        [ForeignKey("ItemId")]
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public int Quantity { get; set; }
    }
}
