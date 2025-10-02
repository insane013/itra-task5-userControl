using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Task5.Models.User;
using Task5.Services.Authentication;
using Task5.Services.Logging;

namespace Task5.WebApp.Controllers;

[Route("Account")]
public class AccountController : BaseController
{

    private readonly IAuthenticationSerivice authService;

    public AccountController(IAuthenticationSerivice authService, ILogger<AccountController> logger)
    : base(logger)
    {
        this.authService = authService;
    }

    [Route("Login")]
    [HttpGet]
    public IActionResult Login()
    {
        return this.View();
    }

    [Route("Login")]
    [HttpPost]
    public async Task<IActionResult> Login(UserLoginDto model)
    {
        if (await this.authService.LoginUser(model))
        {
            return this.RedirectToAction("GetUsersTable", controllerName: "Table");
        }

        return this.View();
    }

    [Route("Logout")]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await this.authService.LogOutUser();

        return this.RedirectToAction("Login", controllerName: "Account");
    }

    [Route("Register")]
    [HttpGet]
    public IActionResult Register()
    {
        return this.View();
    }

    [Route("Register")]
    [HttpPost]
    public async Task<IActionResult> Register([FromForm] UserRegisterDto user)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return this.BadRequest(new { errors });
        }

        var result = await this.authService.RegisterUser(user, this.GetActionUrl("ConfirmEmail", "Account"));

        this.ViewData["Errors"] = result.Errors;

        return result.Succeeded ? this.RedirectToAction("GetUsersTable", "Table") : this.View();
    }

    [Route("ConfirmEmail")]
    [HttpGet]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, string token)
    {
        var result = await this.authService.VerificationComplete(userId, token);
        this._logger.LogInformation($"User: {userId}\nToken: {token}");

        return result ? this.View("ConfirmationSuccess") : this.View("ConfirmationFail");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }

    private string GetActionUrl(string action, string controller)
    {
        var url = Url.Action(
            action: action,
            controller: controller,
            values: null,
            protocol: HttpContext.Request.Scheme,
            host: HttpContext.Request.Host.ToString())
                ?? $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/{controller}/{action}";

        return url;
    }
}