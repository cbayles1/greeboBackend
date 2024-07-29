using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using backend.Models;
using DatabaseVariables;
using Microsoft.AspNetCore.Cors;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    public async Task<IEnumerable<ChocolateBar>> GetAllBars() {
        return await dealer.GetAll();
    }

    [HttpGet("{barId}/cacao")]
    public async Task<int> GetCacaoPercent(int barId) {
        int cacao = await dealer.GetCacaoPercent(barId);
        return cacao;
    }
}
