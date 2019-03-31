using System.Collections.Generic;

namespace BasketApi.Models
{
    public class Basket
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public List<Item> Items { get; set; }
    }
}
