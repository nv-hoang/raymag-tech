using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Localization;
using RefineCMS.Controllers;
using RefineCMS.Helpers;
using RefineCMS.Models;

namespace RefineCMS.Common;

public class Page(ICompositeViewEngine _viewEngine, HookProvider _hook, CFG _cfg)
{
    public Guid Id { get; } = Guid.NewGuid();
    public User? User;
    public Controller Controller = new EmptyController();
    public ControllerActionDescriptor? ActionDescriptor;
    public IStringLocalizer? Localizer;
    public string Language = "en";

    // For Theme
    public BaseModel? PostInstance { get; set; }
    public Dictionary<string, string> MetaPostInstance { get; set; } = [];

    public string? ControllerHook
    {
        get
        {
            if (ActionDescriptor is ControllerActionDescriptor descriptor)
            {
                return $"{descriptor.ControllerName}_{descriptor.ActionName}";
            }
            return null;
        }
    }

    public IActionResult Template(string template)
    {
        if (ControllerHook != null)
        {
            template = _hook.ApplyFilters($"{ControllerHook}_Template", template);
        }

        var templates = new[] {
            $"~/Theme/{_cfg.Theme}/Views/{template}.cshtml",
            $"~/Views/{template}.cshtml"
        };

        var viewPath = templates.FirstOrDefault(p => _viewEngine.GetView(null, p, false).Success) ?? "~/Views/Error.cshtml";

        return Controller.View(viewPath);
    }

    public IActionResult ThemeTemplate(string postType, string postTemplate, int id)
    {
        if (string.IsNullOrEmpty(postTemplate)) postTemplate = "default";

        var templates = new[] {
            $"~/Theme/{_cfg.Theme}/Views/{Slug.ToCamelCase(postType)}_{Slug.ToCamelCase(postTemplate)}_{id}.cshtml",
            $"~/Theme/{_cfg.Theme}/Views/{Slug.ToCamelCase(postType)}_{Slug.ToCamelCase(postTemplate)}.cshtml",
            $"~/Theme/{_cfg.Theme}/Views/{Slug.ToCamelCase(postType)}.cshtml",
        };

        var viewPath = templates.FirstOrDefault(p => _viewEngine.GetView(null, p, false).Success) ?? "~/Views/Error.cshtml";

        return Controller.View(viewPath);
    }
}
