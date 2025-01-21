using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Domain.Entities;
using eCommerce.SharedLibrary.Response;
namespace AuthenticationApi.Application.Interfaces
{
   
public interface IUser
    {
       
        Task<Response> Register(UserDTO appUserDTO);
      
        Task<Response> Login(LoginDTO loginDTO);
        
        Task<GetUserDTO> GetUser(int userId);
        Task<User> GetUserByUserName(string userName);
    }
}