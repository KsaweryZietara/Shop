namespace EventBus.IntegrationEvents {

    public interface ProductPriceChanged {
        public int ProductId { get; set; }

        public decimal NewPrice { get; set; }
    }
}