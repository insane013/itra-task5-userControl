using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Task5.Database.Entities;
using Task5.Models;
using Task5.Models.User;
using Task5.Services.Users;
using Task5.WebApp.Helpers;

namespace Task5.WebApp.Controllers;

[Authorize]
[Route("Table")]
public class TableController : BaseController
{
    private readonly IUserService userService;
    public TableController(IUserService userService, SignInManager<UserEntity> signInManager, ILogger<TableController> logger)
    : base(logger, signInManager)
    {
        this.userService = userService;
    }

    [HttpGet]
    [Route("")]
    [RequireUserId]
    public async Task<IActionResult> GetUsersTable([FromQuery] UserFilter filter)
    {
        if (!this.ValidateModel(out IList<string> errors))
        {
            filter = CookieFilterHelper.LoadFilter<UserFilter>("UserFilter", this.Request);
        }

        return await this.AccessDeniedProceed(async () =>
        {
            var data = await this.userService.GetUserList(this.UserId, filter);

            return this.View("UserTable", data);
        });
    }

    [HttpGet]
    [Route("{page:int}")]
    [RequireUserId]
    public IActionResult GetUsersTablePage(int page)
    {
        var filter = CookieFilterHelper.LoadFilter<UserFilter>("UserFilter", this.Request);

        filter.PageNumber = page;

        return this.RedirectToAction("GetUsersTable", filter);
    }
}