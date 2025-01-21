using Application.Services;
using ProductApi.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class MarketsController : ControllerBase
    {
        private readonly IMarketService _marketService;

        public MarketsController(IMarketService marketService)
        {
            _marketService = marketService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMarkets()
        {
            var markets = await _marketService.GetAllMarketsAsync();
            return Ok(markets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMarketById(long id)
        {
            var market = await _marketService.GetMarketByIdAsync(id);
            if (market == null)
                return NotFound();

            return Ok(market);
        }
        [HttpGet("Owner/{owner}")]
        public async Task<IActionResult> GetMarketByOwner(string owner)
        {
            try
            {
                var marketId = await _marketService.GetMarketByOwnerAsync(owner);
                return Ok(new { MarketId = marketId });
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddMarket([FromBody] MarketDto marketDto)
        {
            var response = await _marketService.AddMarketAsync(marketDto);

            if (!response.Flag)
            {
                return BadRequest(response.Message);
            }

            // Utilisez l'Id généré pour la réponse
            return CreatedAtAction(nameof(GetMarketById), new { id = marketDto.Id }, marketDto);
        }


      /*  [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMarket(long id, [FromBody] MarketDto marketDto)
        {
            if (id != marketDto.Id)
                return BadRequest("Market ID mismatch.");

            await _marketService.UpdateMarketAsync(marketDto);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMarket(long id)
        {
            await _marketService.DeleteMarketAsync(id);
            return NoContent();
        }
      */
        [HttpGet("owner-exists/{owner}")]
        public async Task<bool> DoesOwnerExist(string owner)
        {
            var exists = await _marketService.DoesOwnerExistAsync(owner);
            return exists ;
        }

    }
}
