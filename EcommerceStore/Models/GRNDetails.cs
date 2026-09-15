
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceStore.Models
{

    public class GRNDetails
{
    [Key]
    public int GRNDetailID { get; set; }

    public int GRNID { get; set; }
    public GRNHeader? GRNHeader { get; set; }

    public int ProductID { get; set; }
    public Product? Product { get; set; }

    public int QuantityReceived { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPurchasePrice { get; set; }
}
}