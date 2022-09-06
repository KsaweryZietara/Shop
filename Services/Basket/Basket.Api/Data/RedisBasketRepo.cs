using System.Text.Json;
using Basket.Api.Models;
using StackExchange.Redis;

namespace Basket.Api.Data {

    public class RedisBasketRepo : IBasketRepo {
        private readonly ILogger<RedisBasketRepo> logger;
        private readonly IConnectionMultiplexer redis;
        private readonly IDatabase database;

        public RedisBasketRepo(ILogger<RedisBasketRepo> logger, IConnectionMultiplexer redis){
            this.logger = logger;
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
                logger.LogInformation("Problem occur persisting the item.");
                return null;
            }

            logger.LogInformation("Basket item persisted succesfully.");

            return await GetBasketByIdAsync(basket.BuyerId);
        }

        public Task<bool> DeleteBasketAsync(string id) {
            var deleted = database.KeyDeleteAsync(id);

            return deleted;
        }

        public IEnumerable<string> GetBuyers() {
            var endpoint = redis.GetEndPoints();
            var server = redis.GetServer(endpoint.First());
            var users = server.Keys();

            return users.Select(u => u.ToString());
        }
    }
}