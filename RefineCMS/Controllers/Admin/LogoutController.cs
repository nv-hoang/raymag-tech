using Microsoft.AspNetCore.Mvc;
using RefineCMS.Attributes;
using RefineCMS.Common;

namespace RefineCMS.Controllers.Admin;

[AdminController("logout")]
[Auth]
public class LogoutController(AppFactory _appFactory) : BaseController(_appFactory)
{
    [HttpGet]
    public IActionResult Index()
    {
        HttpContext.Session.Remove("user");
        return RedirectToAction("Index", "Login");
    }
}
