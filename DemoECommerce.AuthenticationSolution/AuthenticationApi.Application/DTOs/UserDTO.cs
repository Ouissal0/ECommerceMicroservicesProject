using AuthenticationApi.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationApi.Application.DTOs
{
    public record UserDTO
    (
        int Id,
        [Required] string FirstName,
        [Required] string LastName,
        [Required, EmailAddress] string Email,
        [Required] string Password,
        [Required] string PhoneNumber,
        [Required] string UserName,
        [Required] string image,
        [Required] Role Role
    );
}
