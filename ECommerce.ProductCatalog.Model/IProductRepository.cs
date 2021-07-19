﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.ProductCatalog.Model
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task AddProduct(Product product);
    }
}
