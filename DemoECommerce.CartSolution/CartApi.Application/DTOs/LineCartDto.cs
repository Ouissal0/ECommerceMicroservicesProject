using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartApi.Application.DTOs
{
    public record CartLineDto(
           long Id,
           int Quantity,
           double SubTotal,
           long ProductId
       );
}
