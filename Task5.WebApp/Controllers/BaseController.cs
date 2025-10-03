using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Task5.Database.Entities;
using Task5.Services.Authentication;
using Task5.Services.Exceptions;
using Task5.Services.Logging;

namespace Task5.WebApp.Controllers;

public abstract class BaseController : Controller
{
    protected readonly ILogger<BaseController> Logger;
    private readonly SignInManager<UserEntity> signInManager;

    protected BaseController(ILogger<BaseController> logger, SignInManager<UserEntity> signInManager)
    {
        this.Logger = logger;
        this.signInManager = signInManager;
    }

    protected string? UserId
    {
        get => this.HttpContext?.Items["UserId"]?.ToString();
    }

    /// <summary>
    /// Gets Id of currently authorized user.
    /// </summary>
    /// <returns>User ID.</returns>
    /// <exception cref="UnauthorizedAccessException">Throws if there is no authorized users.</exception>
    protected string GetUserIdOrThrow()
    {
        var userId = this.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            CommonLogs.LogWarningMessage(this.Logger, $"Unauthorized user access.");
            throw new UnauthorizedAccessException("Unauthorized user access.");
        }

        return userId;
    }

    protected async Task<IActionResult> AccessDeniedProceed(Func<Task<IActionResult>> func)
    {
        try
        {
            return await func.Invoke();
        }
        catch (AccessDeniedException)
        {
            CommonLogs.LogWarningMessage(this.Logger, "Unauthorized user access.");
            await this.signInManager.SignOutAsync();
            this.TempData["Error"] = "Your account is currently blocked.";
            return this.Forbid();
        }
    }

    protected bool ValidateModel(out IList<string> errors)
    {
        if (!this.ModelState.IsValid)
        {
            errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return false;
        }
        errors = Enumerable.Empty<string>().ToList();
        return true;
    }
}