using Microsoft.AspNetCore.Mvc;
using RefineCMS.Attributes;
using RefineCMS.Common;

namespace RefineCMS.Controllers.Admin;

[AdminController]
[Auth]
public class DashboardController(AppFactory _appFactory) : BaseController(_appFactory)
{
    [HttpGet("")]
    public IActionResult Index()
    {
        return _page.Template("Admin/Dashboard");
    }
}
