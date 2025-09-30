using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Task5.Services.Abstraction;

namespace Task5.WebApp.Controllers;

[Route("Table")]
public class TableController : BaseController
{
    private readonly IUserService userService;
    public TableController(IUserService userService) : base()
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