namespace BasketApi.Models
{
    public class BasketItem
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public int Quantity { get; set; }
    }
}
