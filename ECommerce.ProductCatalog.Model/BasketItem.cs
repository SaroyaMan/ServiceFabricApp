using System;

namespace ECommerce.ProductCatalog.Model
{
    public class BasketItem
    {
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
