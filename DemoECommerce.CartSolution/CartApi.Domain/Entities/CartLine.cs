using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartApi.Domain.Entities
{
    public class CartLine
    {
        public long Id { get; set; }
        public int Quantity { get; set; }
        public double SubTotal { get; set; }
        public long ProductId { get; set; } // Reference to the product microservice
        public long CartId { get; set; } // Foreign key to the cart

        // Method to calculate the subtotal
        public void CalculateSubTotal(double unitPrice)
        {
            SubTotal = Quantity * unitPrice;
        }

        // Method to modify the quantity
        public void ModifyQuantity(int newQuantity, double unitPrice)
        {
            Quantity = newQuantity;
            CalculateSubTotal(unitPrice);
        }
    }
}
