using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceStore.Models
{
    public class SalesOrder
    {
        [Key]
        public int OrderID { get; set; }

        public int CustomerID { get; set; }
        public User? Customer { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public string OrderStatus { get; set; } = "Pending";

        public ICollection<SalesOrderDetails>? OrderDetails { get; set; }
    }
}