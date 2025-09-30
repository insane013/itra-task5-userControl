using Microsoft.AspNetCore.Mvc;

namespace Task5.WebApp.Controllers;

public class HomeController : BaseController
{
    [HttpGet]
    [Route("/{name}")]
    public IActionResult Home(string name)
    {
        return this.View();
    }
}