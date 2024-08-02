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
    public async Task<float[]> GetGreebloStatus([FromQuery] string schedule) {
        int[] barIds = ParseScheduleString(schedule);
        return await dealer.GetStatus(barIds);
    }

    private static int[] ParseScheduleString(string schedule) {
        if (string.IsNullOrEmpty(schedule)) {
            throw new Exception("Schedule query parameter is required.");
        }
        // Splits by comma, parse as ints, put back into array
        int[] arr = schedule.Split(',').Select(int.Parse).ToArray();
        return arr;
    }
}
