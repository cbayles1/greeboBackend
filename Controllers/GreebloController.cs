using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using backend.Models;
using DatabaseVariables;

namespace backend.Controllers;

[ApiController]
[Route("api/schedule")]
public class GreebloController : ControllerBase {
    GreebloDealer dealer;

    public GreebloController(IOptions<DatabaseCredentials> credentials) {
        dealer = new GreebloDealer(credentials.Value);
    }

    [HttpGet("")]
    public async Task<double> GetGreebloStatus([FromQuery] string scheduleStr) {
        int[] schedule = scheduleStr.Split(',').Select(int.Parse).ToArray();
        // TODO: PREVENT DUPLICATES
        return await dealer.GetStatus(schedule);
    }
}
