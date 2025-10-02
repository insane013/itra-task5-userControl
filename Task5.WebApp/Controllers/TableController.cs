using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task5.Services.Users;

namespace Task5.WebApp.Controllers;

[Authorize]
[Route("Table")]
public class TableController : BaseController
{
    private readonly IUserService userService;
    public TableController(IUserService userService, ILogger<TableController> logger) : base(logger)
    {
        this.userService = userService;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetUsersTable()
    {
        var data = await this.userService.GetUserList();

        return this.View("UserTable", data);
    }
}