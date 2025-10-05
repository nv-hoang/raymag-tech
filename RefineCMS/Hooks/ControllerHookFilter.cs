using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using RefineCMS.Attributes;
using RefineCMS.Common;
using RefineCMS.Common.UI;
using RefineCMS.Extensions;
using RefineCMS.Models;
using System.Globalization;

namespace RefineCMS.Hooks;

public class ControllerHookFilter(Page _page, HookProvider _hook, AppFactory _appFactory, IStringLocalizerFactory _factory) : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        _page.Localizer = _factory.Create("Language", typeof(Language).Assembly.GetName().Name!);
        _page.Language = CultureInfo.CurrentUICulture.Name;

        _page.User = context.HttpContext.Session.GetObject<User>("user");

        if (context.Controller is Controller controller)
        {
            _page.Controller = controller;
        }

        if (context.ActionDescriptor is ControllerActionDescriptor descriptor)
        {
            var hidden = _page.Controller.GetType().GetProperty("Hidden")?.GetValue(_page.Controller) as List<string>;
            if (hidden != null && hidden.Contains(descriptor.ActionName))
            {
                context.Result = new NotFoundResult();
                return;
            }

            _page.ActionDescriptor = descriptor;

            if (_page.User != null && _page.User.UserLogin != "admin")
            {
                var controllerType = _page.Controller.GetType();
                List<string> excludes = ["Login", "Dashboard", "Api", "Storage"];
                if (Attribute.IsDefined(controllerType, typeof(AdminControllerAttribute)) && !excludes.Contains(descriptor.ControllerName))
                {
                    var urls = (new Sidebar(_page.Controller.Url, _appFactory)).GetUserUrls(_page.User);
                    if (!urls.Any(x => context.HttpContext.Request.Path.ToString().StartsWith(x)))
                    {
                        context.Result = new NotFoundResult();
                        return;
                    }
                }
            }

            _hook.DoAction($"{_page.ControllerHook}");
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Optional: logic after action executes
    }
}
