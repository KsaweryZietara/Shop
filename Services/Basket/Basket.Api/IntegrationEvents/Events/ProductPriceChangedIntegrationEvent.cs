namespace Basket.Api.IntegrationEvents.Events {

    public interface ProductPriceChanged {
        public int ProductId { get; set; }

        public decimal NewPrice { get; set; }
    }
}