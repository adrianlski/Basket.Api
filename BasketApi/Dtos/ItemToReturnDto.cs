namespace BasketApi.Dtos
{
    public class ItemToReturnDto
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal ItemPrice { get; set; }
    }
}
