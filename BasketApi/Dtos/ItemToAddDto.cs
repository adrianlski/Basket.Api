using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketApi.Dtos
{
    public class ItemToAddDto
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }
}
