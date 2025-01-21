using CartApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartApi.Infrastructure.Data
{
    public class CartDbContext(DbContextOptions<CartDbContext> options) : DbContext
    {
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartLine> CartLines { get; set; }
       
    }
}
