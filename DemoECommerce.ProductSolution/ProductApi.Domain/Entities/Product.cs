

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductApi.Domain.Entities
{
    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int StockQuantity { get; set; }

        [Column(TypeName = "text")] // Force le type Text en base de données
        public string Image { get; set; }

        // Foreign key and navigation property for Category
        public long CategoryId { get; set; }
        public Category? Category { get; set; }

        // Foreign key and navigation property for Market
        public long MarketId { get; set; }
        public Market? Market { get; set; }
    }
}
