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
            return this.RedirectToAction("GetusersTable", controllerName: "Table");
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

        await this.authService.RegisterUser(user, this.GetActionUrl("ConfirmEmail", "Account"));

        return View();
    }

    [Route("ConfirmEmail")]
    public IActionResult ConfirmEmail([FromQuery] string userId, string token)
    {
        return this.View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }

    private string GetActionUrl(string action, string controller)
    {
        var url = Url.Action(action: action, controller: controller)
                ?? $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/{controller}/{action}";

        return url;
    }
}