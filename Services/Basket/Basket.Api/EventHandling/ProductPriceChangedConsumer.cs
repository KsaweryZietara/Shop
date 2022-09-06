using Basket.Api.Data;
using EventBus.IntegrationEvents;
using MassTransit;

namespace Basket.Api.EventHandling {

    public class ProductPriceChangedConsumer : IConsumer<ProductPriceChanged> {
        private readonly ILogger<ProductPriceChangedConsumer> logger;
        private readonly IBasketRepo repository;

        public ProductPriceChangedConsumer(ILogger<ProductPriceChangedConsumer> logger, IBasketRepo repository) {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<ProductPriceChanged> context) {
            logger.LogInformation("Handling integration event: {eventName} at {appName}",
                "ProductPriceChanged", "Basket.Api");

            var buyers = repository.GetBuyers();

            foreach(var key in buyers) {
                
                var basket = await repository.GetBasketByIdAsync(key);

                foreach(var item in basket.Items) {
                    if(item.ProductId == context.Message.ProductId) {
                        logger.LogInformation("Updating item in basket for user: {userId} ({itemId})", key, item.Id);
                        item.Price = context.Message.NewPrice;
                    }
                }

                await repository.UpdateBasketAsync(basket);
            }
        }
    }

    public class ProductPriceChangedConsumerDefinition : ConsumerDefinition<ProductPriceChangedConsumer> {
        public ProductPriceChangedConsumerDefinition() {
            EndpointName = "new-price-service";
            ConcurrentMessageLimit = 8;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, 
            IConsumerConfigurator<ProductPriceChangedConsumer> consumerConfigurator) {
            
            endpointConfigurator.UseMessageRetry(r => r.Intervals(100,200,500,800,1000));

            endpointConfigurator.UseInMemoryOutbox();
        }
    }
}