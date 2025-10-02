using Microsoft.AspNetCore.Mvc;

namespace Task5.WebApp.Controllers;

public abstract class BaseController : Controller
{
    protected readonly ILogger<BaseController> _logger;

    protected BaseController(ILogger<BaseController> logger)
    {
        this._logger = logger;
    }
}