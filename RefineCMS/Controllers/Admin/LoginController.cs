using Microsoft.AspNetCore.Mvc;
using RefineCMS.Attributes;
using RefineCMS.Common;
using RefineCMS.Extensions;
using RefineCMS.Models;
using RefineCMS.Providers;
using RefineCMS.Requests;

namespace RefineCMS.Controllers.Admin;

[AdminController("login")]
[Guest]
public class LoginController(AppFactory _appFactory) : BaseController(_appFactory)
{
    protected readonly ModelProvider<User> _userProvider = _appFactory.Create<ModelProvider<User>>();

    [HttpGet]
    public IActionResult Index()
    {
        return _page.Template("Admin/Login");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(LoginRequest loginRequest)
    {
        var user = _userProvider.FirstOrDefault(u => u.UserLogin == loginRequest.Username && u.UserStatus == 1);

        if (user != null && user.VerifyPassword(loginRequest.Password))
        {
            _hook.DoAction("Login_Success", user);

            HttpContext.Session.SetObject("user", user);
            return RedirectToAction("Index", "Dashboard");
        }

        _hook.DoAction("Login_Failed", user?.Id ?? 0);

        TempData["MessageError"] = _output.Trans("Username or password is incorrect.");
        ViewData["Username"] = loginRequest.Username;
        return _page.Template("Admin/Login");
    }
}
