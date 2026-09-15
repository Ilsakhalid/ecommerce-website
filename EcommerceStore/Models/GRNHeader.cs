using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceStore.Models
{
    public class GRNHeader
    {
        [Key]
        public int GRNID { get; set; }

        [Required]
        public string SupplierName { get; set; } = string.Empty;

        public DateTime ReceivedDate { get; set; } = DateTime.Now;

        public int AdminUserID { get; set; }
        public User? AdminUser { get; set; }

        public ICollection<GRNDetails>? GRNDetailsList { get; set; }
    }
}