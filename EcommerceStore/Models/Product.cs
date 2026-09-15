using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceStore.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string PackSize { get; set; } = string.Empty;

        [Required]
        public int StockQuantity { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        // Added ImageURL property for product pictures
        public string? ImageURL { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Foreign Keys
        public int CategoryID { get; set; }
        public Category? Category { get; set; }

        public int NatureTypeID { get; set; }
        public BusinessNatureType? BusinessNatureType { get; set; }
    }
}