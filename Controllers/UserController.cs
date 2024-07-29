using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using backend.Models;
using DatabaseVariables;

namespace backend.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase {
    UserDealer dealer;

    public UsersController(IOptions<DatabaseCredentials> credentials) {
        dealer = new UserDealer(credentials.Value);
    }

    [HttpGet("{barId}")]
    public async Task<User> GetBar(int barId) {
        return await dealer.Get(barId);
    }
}
