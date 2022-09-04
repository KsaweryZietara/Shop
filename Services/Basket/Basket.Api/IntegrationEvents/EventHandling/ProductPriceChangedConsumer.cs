using Basket.Api.Data;
using EventBus.IntegrationEvents;
using MassTransit;

namespace Basket.Api.IntegrationEvents.EventHandling {

    public class ProductPriceChangedConsumer : IConsumer<ProductPriceChanged> {
        private readonly IBasketRepo repository;

        public ProductPriceChangedConsumer(IBasketRepo repository) {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<ProductPriceChanged> context) {
            Console.WriteLine($"Product-{context.Message.ProductId} new price: {context.Message.NewPrice}");

            
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