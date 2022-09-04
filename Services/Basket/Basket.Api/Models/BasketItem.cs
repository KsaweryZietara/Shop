using System.ComponentModel.DataAnnotations;

namespace Basket.Api.Models {

    public class BasketItem {

        [Required]
        public string? Id { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(50)]
        public string? ProductName { get; set; }

        [Required]
        [Range(0, Double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
    }
}