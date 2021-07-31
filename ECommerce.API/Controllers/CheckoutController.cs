using ECommerce.API.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ECommerce.CheckoutService.Model;
using System.Linq;
using ECommerce.ProductCatalog.Model;
using System.Collections.Generic;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using Microsoft.ServiceFabric.Services.Client;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckoutController: ControllerBase
    {
        private static readonly Random rnd = new Random(DateTime.UtcNow.Second);

        [Route("{userId}")]
        public async Task<ApiCheckoutSummary> CheckoutAsync(string userId)
        {
            CheckoutSummary summary = await GetCheckoutService().CheckoutAsync(userId);

            return ToApiCheckoutSummary(summary);
        }

        [Route("history/{userId}")]
        public async Task<IEnumerable<ApiCheckoutSummary>> GetHistoryAsync(string userId)
        {
            var history = await GetCheckoutService().GetOrderHistoryAsync(userId);

            return history.Select(ToApiCheckoutSummary);
        }

        private ICheckoutService GetCheckoutService()
        {
            long key = LongRandom();

            var proxyFactory = new ServiceProxyFactory(c => new FabricTransportServiceRemotingClientFactory());

            return proxyFactory.CreateServiceProxy<ICheckoutService>(new Uri("fabric:/ECommerce/ECommerce.CheckoutService"), new ServicePartitionKey(key));
        }

        private long LongRandom()
        {
            byte[] buf = new byte[8];
            rnd.NextBytes(buf);

            return BitConverter.ToInt64(buf, 0);
        }

        private ApiCheckoutSummary ToApiCheckoutSummary(CheckoutSummary summary)
        {
            return new ApiCheckoutSummary
            {
                Products = summary.Products.Select(p => new ApiCheckoutProduct
                { 
                    ProductId = p.Product.Id,
                    ProductName = p.Product.Name,
                    Price = p.Price,
                    Quantity = p.Quantity
                }).ToList(),
                Date = summary.Date,
                TotalPrice = summary.TotalPrice
            };
        }
    }
}
