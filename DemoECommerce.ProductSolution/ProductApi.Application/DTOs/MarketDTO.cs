using System.ComponentModel.DataAnnotations;

namespace ProductApi.Application.DTOs
{
    public class MarketDto
    {
        public long Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Name must be less than 100 characters.")]
        public string Name { get; set; }
        public string Owner { get; set; }
        public string Description { get; set; }

        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180.")]
        public double Longitude { get; set; }

        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90.")]
        public double Latitude { get; set; }

        [Required(ErrorMessage = "Image is required.")]
        public string Image { get; set; }
        public string PhoneNumber { get; set; }

    }
}
