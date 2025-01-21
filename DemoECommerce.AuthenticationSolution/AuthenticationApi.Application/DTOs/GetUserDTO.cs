using AuthenticationApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Application.DTOs
{
    public record class GetUserDTO
    (
        int Id,

      [Required] string firstName,

      [Required] string lastName,

      [Required, EmailAddress] string email,

      [Required] string phoneNumber,

      [Required] string image ,

      [Required] string userName,

      [Required] Role role
    );
}
