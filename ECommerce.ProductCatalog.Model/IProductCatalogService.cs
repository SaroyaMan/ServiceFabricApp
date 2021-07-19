using Microsoft.ServiceFabric.Services.Remoting;
using System.Threading.Tasks;

namespace ECommerce.ProductCatalog.Model
{
    public interface IProductCatalogService : IService
    {
        Task<Product[]> GetAllProductsAsync();
        Task AddProductAsync(Product product);
    }
}
