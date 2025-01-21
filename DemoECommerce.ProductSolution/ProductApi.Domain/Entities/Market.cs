

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProductApi.Domain.Entities
{
    public class Market
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        public string Owner { get; set; }

        public double Longitude { get; set; }

        public string PhoneNumber { get; set; }
        public double Latitude { get; set; }

        [Column(TypeName = "text")] // Force le type Text en base de données
        public string Image { get; set; }

        // Navigation property for related products
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
