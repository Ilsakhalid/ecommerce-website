using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EcommerceStore.Models
{
    public class SalesOrderDetails
    {
        [Key]
        public int OrderDetailID { get; set; }

        public int OrderID { get; set; }
        public SalesOrder? SalesOrder { get; set; }

        public int ProductID { get; set; }
        public Product? Product { get; set; }

        public int QuantitySold { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
    }
}