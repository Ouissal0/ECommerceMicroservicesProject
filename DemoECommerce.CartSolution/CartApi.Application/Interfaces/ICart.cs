using CartApi.Domain.Entities;
using eCommerce.SharedLibrary.Interface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CartApi.Application.Interfaces
{
    public interface ICart //: IGenericInterface<Cart>
    {



        Task<Cart?> GetCartAsync(int cartId);
        Task UpdateCartAsync(Cart cart);
         Task<Cart> AddAsync(Cart cart);
        Task DeleteCartAsync(int cartId);


    }
}
