using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Domain.Entities;
using AuthenticationApi.Infrastructure.Data;
using eCommerce.SharedLibrary.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AuthenticationApi.Infrastructure.Repositories
{

    internal class UserRepository(AuthenticationDbContext context,IConfiguration config) : IUser
    {
        public async Task<User> GetUserByUserName(string userName)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.userName == userName);
            return user is null ? null! : user!; 
        }

        public async Task<GetUserDTO> GetUser(int userId)
        {
            var user = await context.Users.FindAsync(userId);
            return user is not null ? new GetUserDTO(user.Id!,
            user.firstName!,
            user.lastName!,
            user.userName!,
            user.phoneNumber!,
            user.image!,
            user.email!,
            user.role!) : null!;
        }

        public async Task<Response> Login(LoginDTO loginDTO)
        {
            var getUser = await GetUserByUserName(loginDTO.userName);
            if(getUser is null)
                return new Response(false,"Invalid credentials");

            bool verifyPassword=BCrypt.Net.BCrypt.Verify(loginDTO.password,getUser.password);
            if (!verifyPassword)
            {
                return new Response(false, "Invalid credentials");
            }
            string token = GenerateToken(getUser);
            return new Response(true, token);
        }

        public  string GenerateToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(config.GetSection("Authentication:Key").Value!);
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name,user.userName!),
                new(ClaimTypes.Email,user.email!)

            };
            var token = new JwtSecurityToken(issuer: config["Authentication:Issuer"],
                audience: config["Authentication:Audience"],
                claims: claims,
                expires: null,
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<Response> Register(UserDTO userDTO)
        {
            var getUser = await GetUserByUserName(userDTO.UserName);
            if (getUser is not null)
            {
                return new Response(false, $"You can not use this UserName for Registration");
            }
            var result = context.Users.Add(new User()
            {
                firstName = userDTO.FirstName,
                lastName = userDTO.LastName,
                userName = userDTO.UserName,
                phoneNumber = userDTO.PhoneNumber,
                email = userDTO.Email,
                image=userDTO.image,
                role = userDTO.Role,
                password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password)

            });

            await context.SaveChangesAsync();
            return result.Entity.Id > 0 ? new Response(true, "User Registered Successfully") :
            new Response(false, "Invalide data provided");
        }
    }

}