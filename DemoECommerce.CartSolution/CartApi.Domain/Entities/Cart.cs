using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartApi.Domain.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public double TotalAmount { get; set; }
        public long UserId { get; set; } // Reference to the user microservice
        public List<CartLine> CartLines { get; set; } = new List<CartLine>();

        // Method to calculate the total amount
        public void CalculateTotal()
        {
            TotalAmount = 0;
            foreach (var line in CartLines)
            {
                TotalAmount += line.SubTotal;
            }
        }

        // Method to empty the cart
        public void Clear()
        {
            CartLines.Clear();
            TotalAmount = 0;
        }

        // Method to validate the cart
        public bool Validate()
        {
            return CartLines.Count > 0;
        }
    }
}


