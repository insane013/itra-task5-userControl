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

    [HttpPost]
    [Route("BlockUsers")]
    [RequireUserId]
    public Task<IActionResult> BlockUsers([FromForm] IEnumerable<string> emailList) =>
        this.EmailListProceed(emailList, this.userService.BlockUsers, "Users blocking procedure initiated..");

    [HttpPost]
    [Route("UnblockUsers")]
    [RequireUserId]
    public Task<IActionResult> UnblockUsers([FromForm] IEnumerable<string> emailList) =>
        this.EmailListProceed(emailList, this.userService.UnblockUsers, "Users unblocking procedure initiated..");

    [HttpPost]
    [Route("DeleteUsers")]
    [RequireUserId]
    public Task<IActionResult> DeleteUsers([FromForm] IEnumerable<string> emailList) =>
        this.EmailListProceed(emailList, this.userService.DeleteUsers, "Users deleting procedure initiated..");

    [HttpPost]
    [Route("DeleteUnverified")]
    [RequireUserId]
    public async Task<IActionResult> DeleteUnverified()
    {
        this.Logger.LogInformation("Deleting all unverified users.");

        return await this.AccessDeniedProceed(async () =>
        {
            await this.userService.DeleteUnverifiedUsers(UserId);

            return this.RedirectToAction("GetUsersTable");
        });
    }

    private async Task<IActionResult> EmailListProceed(
                    IEnumerable<string> emailList,
                    Func<string?, IEnumerable<string>, Task> action,
                    string logMessage)
    {
        if (!emailList.Any())
            return this.RedirectToAction("GetUsersTable");

        this.Logger.LogInformation(logMessage);

        return await this.AccessDeniedProceed(async () =>
        {
            await action(this.UserId, emailList);

            return this.RedirectToAction("GetUsersTable");
        });
    }
}