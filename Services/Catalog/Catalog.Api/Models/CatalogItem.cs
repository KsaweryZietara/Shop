using System.ComponentModel.DataAnnotations;

namespace Catalog.Api.Models {

    public class CatalogItem {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(300)]
        public string? Description { get; set; }

        [Required]
        [Range(0, Double.MaxValue)]
        public decimal Price { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string? Brand { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int AvailableStock { get; set; }
    }
}