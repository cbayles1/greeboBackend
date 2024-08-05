using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using backend.Models;
using DatabaseVariables;

namespace backend.Controllers;

[ApiController]
[Route("api/chocolateBars")]
public class ChocolateBarsController : ControllerBase {
    ChocolateBarDealer dealer;

    public ChocolateBarsController(IOptions<DatabaseCredentials> credentials) {
        dealer = new ChocolateBarDealer(credentials.Value);
    }

    [HttpGet("{barId}")]
    public async Task<ChocolateBar> GetBar(int barId) {
        return await dealer.Get(barId);
    }

    [HttpGet("")]
    public async Task<IEnumerable<ChocolateBar>> GetAllBars([FromQuery] int limit) {
        return await dealer.GetAll(limit);
    }
}
