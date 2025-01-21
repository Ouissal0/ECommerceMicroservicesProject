using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartApi.Application.DTOs
{
    public record CartDto(
      int Id,
      DateTime CreatedAt,
      double TotalAmount,
      long UserId,
      List<CartLineDto> CartLines
  );
}
