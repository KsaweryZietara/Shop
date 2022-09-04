using Basket.Api.Models;

namespace Basket.Api.Data {
    public interface IBasketRepo {
        Task<CustomerBasket?> GetBasketByIdAsync(string id);
        Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket);
        Task<bool> DeleteBasketAsync(string id);
    }
}