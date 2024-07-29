using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using backend.Models;
using DatabaseVariables;
using Npgsql;

namespace backend.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase {
    UserDealer userDealer;
    ChocolateBarDealer chocoDealer;

    public UsersController(IOptions<DatabaseCredentials> credentials) {
        userDealer = new UserDealer(credentials.Value);
        chocoDealer = new ChocolateBarDealer(credentials.Value);
    }

    [HttpGet("{userId}")]
    public async Task<User> GetUser(int userId) {
        return await userDealer.Get(userId);
    }

    // [HttpGet("{userId}/bars")]
    // public async Task<int[]> GetBars(int userId) {
    //     return await dealer.GetBars(userId);
    // }

    [HttpGet("{userId}/bars")]
    public async Task<ChocolateBar[]> GetBars(int userId) {
        int[] barIds = await userDealer.GetBarIds(userId);
        if (barIds == null) {
            throw new Exception("Response not understood.");
        }

        List<ChocolateBar> chocolateBars = [];
        foreach (int id in barIds) {
            chocolateBars.Add(await chocoDealer.Get(id));
        }
        
        return chocolateBars.ToArray();
    }
}