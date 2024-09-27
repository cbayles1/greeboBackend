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
    public async Task<Greeblo> GetGreebloStatus([FromQuery] string scheduleStr) {
        int[] schedule = scheduleStr.Split(',').Select(int.Parse).ToArray();
        
        // foreach (var number in schedule.GroupBy(x => x)) { // PREVENT DUPLICATES
        //     if (number.Count() > 1 && number.Key >= 0) {
        //         throw new Exception("No duplicates allowed!");
        //     }
        // }
        
        return await dealer.GetStatus(schedule);
    }
}
