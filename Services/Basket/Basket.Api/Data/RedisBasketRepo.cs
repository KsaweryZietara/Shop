using System.Text.Json;
using Basket.Api.Models;
using StackExchange.Redis;

namespace Basket.Api.Data {

    public class RedisBasketRepo : IBasketRepo {
        private readonly IConnectionMultiplexer redis;
        private readonly IDatabase database;

        public RedisBasketRepo(IConnectionMultiplexer redis){
            this.redis = redis;
            database = this.redis.GetDatabase();
        }

        public async Task<CustomerBasket?> GetBasketByIdAsync(string id) {
            var basket = await database.StringGetAsync(id);

            if(basket.IsNullOrEmpty) {
                return null;
            }

            return JsonSerializer.Deserialize<CustomerBasket>(basket);
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket) {
            var basketString = JsonSerializer.Serialize(basket);

            var updated = await database.StringSetAsync(basket.BuyerId, basketString);

            if(!updated) {
                return null;
            }

            return await GetBasketByIdAsync(basket.BuyerId);
        }

        public Task<bool> DeleteBasketAsync(string id) {
            var deleted = database.KeyDeleteAsync(id);

            return deleted;
        }
    }
}