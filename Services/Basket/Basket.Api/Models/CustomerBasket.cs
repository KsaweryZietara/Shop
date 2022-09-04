namespace Basket.Api.Models {

    public class CustomerBasket {

        public string? BuyerId { get; set; }

        public List<BasketItem> Items { get; set; } = new();
    }
}