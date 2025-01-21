using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Domain.Entities;
using eCommerce.SharedLibrary.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AuthenticationApi.Presentation.Controllers
{
    [Route("api/Authentication")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController(IUser userInterface) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<Response>> Register([FromBody] UserDTO userDTO)
        {
            try
            {
                if (userDTO == null || !ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                    Console.WriteLine("Validation errors: " + string.Join(", ", errors));
                    return BadRequest(new { Errors = errors });
                }

                Console.WriteLine("Registering user: " + JsonSerializer.Serialize(userDTO));

                var result = await userInterface.Register(userDTO);
                return result.Flag ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception during registration: " + ex.Message);
                return StatusCode(500, new { Title = "Error", Status = 500, Detail = "Sorry, Internal server error occurred. Kindly try again." });
            }
        }


        [HttpPost("login")]

        public async Task<ActionResult<Response>> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });
            var result=await userInterface.Login(loginDTO);
            return result.Flag ? Ok(result) : BadRequest(Request);
        }

        [HttpGet("user/{username}")]
        [Authorize]
        public async Task<ActionResult<UserDTO>> GetUserByUsername(string username)
        {
            try
            {
                var user = await userInterface.GetUserByUserName(username);
                if (user == null)
                    return NotFound(new { Message = $"User with username '{username}' not found" });

                var userDTO = new UserDTO(
                    user.Id,
                    user.firstName,
                    user.lastName,
                    user.email,
                    user.password,
                    user.phoneNumber,
                    user.userName,
                    user.image,
                    user.role
                );

                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Title = "Error", Detail = "Sorry, Internal server error occurred. Kindly try again." });
            }
        }

        [HttpGet("user/id/{id}")]
        [Authorize]
        public async Task<ActionResult<GetUserDTO>> GetUserById(int id)
        {
            try
            {
                var user = await userInterface.GetUser(id);
                if (user == null)
                    return NotFound(new { Message = $"User with ID '{id}' not found" });

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Title = "Error", Detail = "Sorry, Internal server error occurred. Kindly try again." });
            }
        }
        [HttpGet("user/role/{username}")]
        [Authorize]
        public async Task<Boolean> GetUserRole(string username)
        {
            var user = await userInterface.GetUserByUserName(username);
            if (user.role == Role.VENDEUR)
                return true;
            else
                return false;

        }
    }
}
