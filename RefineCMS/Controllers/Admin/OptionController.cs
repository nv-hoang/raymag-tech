using Microsoft.AspNetCore.Mvc;
using RefineCMS.Attributes;
using RefineCMS.Common;
using RefineCMS.Common.UI;
using RefineCMS.Models;
using RefineCMS.Providers;
using RefineCMS.Requests;

namespace RefineCMS.Controllers.Admin;

[AdminController("options")]
[Auth]
public class OptionController(AppFactory _appFactory) : BaseController(_appFactory)
{
    protected readonly ModelProvider<Option> _optionProvider = _appFactory.Create<ModelProvider<Option>>();
    protected string OptionType { get; set; } = string.Empty;

    protected bool ValidateOptionType(string type)
    {
        List<KeyValuePair<string, string>> list = [];
        list = _hook.ApplyFilters("Theme_Options", list);

        var option = list.FirstOrDefault(x => x.Key == type);
        if (!option.Equals(default(KeyValuePair<string, string>)))
        {
            OptionType = type;
            ViewData["Title"] = _output.Trans(option.Value);
            return true;
        }

        return false;
    }

    [HttpGet("{type}")]
    public IActionResult Index(string type)
    {
        if (!ValidateOptionType(type)) return NotFound();

        var formUI = GetFormUI();
        ViewData["FormUI"] = formUI;
        return _page.Template("Admin/FormView");
    }

    [HttpPost("{type}")]
    public virtual IActionResult IndexFormSubmit(string type, FormUIRequest request)
    {
        if (!ValidateOptionType(type)) return NotFound();

        var formUI = GetFormUI();
        formUI.SetData(request.FormData);
        if (formUI.IsValid())
        {
            return (IActionResult)formUI.DoAction(request.SubmitAction)!;
        }

        TempData["MessageError"] = _output.Trans("Invalid request, please check again.");
        ViewData["FormUI"] = formUI;
        return _page.Template("Admin/FormView");
    }

    protected FormUI GetFormUI()
    {
        var _ = _output.Trans;
        var form = new FormUI
        {
            Actions = [
                new FormAction {
                    Name = "save",
                    Label = _("Save Changes"),
                    OnAction = (form) => {
                        UpdateOption(form);

                        _page.Controller.TempData["MessageSuccess"] = _("Saved successfully!");
                        return _page.Controller.RedirectToAction("Index", new { type = OptionType });
                    }
                },
            ]
        };

        form = _hook.ApplyFilters("Theme_Options_FormUI", form, OptionType);

        var meta = _optionProvider.GetOption(OptionType);
        foreach (var field in form.Fields)
        {
            field.LoadModelData(null, meta);
        }

        return form;
    }

    protected void UpdateOption(FormUI form)
    {
        var meta = form.ToMeta();
        _optionProvider.UpdateOption(OptionType, meta);
    }
}
