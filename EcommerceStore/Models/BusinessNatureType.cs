using System.ComponentModel.DataAnnotations;

namespace EcommerceStore.Models
{
    public class BusinessNatureType
    {
        [Key]
        public int NatureTypeID { get; set; }

        [Required, StringLength(100)]
        public string TypeName { get; set; } = string.Empty;

        public ICollection<Product>? Products { get; set; }
    }
}