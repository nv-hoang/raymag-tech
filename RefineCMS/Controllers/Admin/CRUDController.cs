using Microsoft.AspNetCore.Mvc;
using RefineCMS.Common;
using RefineCMS.Common.UI;
using RefineCMS.Requests;

namespace RefineCMS.Controllers.Admin;
public class CRUDController(AppFactory _appFactory) : BaseController(_appFactory)
{
    protected FormUIRequest? FormRequest;

    [HttpGet]
    public virtual IActionResult Index()
    {
        var tableUI = GetTableUI();
        ViewData["TableUI"] = tableUI;
        ViewData["Title"] = _output.Trans(PluralPageTitle);
        return _page.Template("Admin/ListView");
    }

    [HttpGet("list")]
    public virtual IActionResult ApiList(TableUIRequest request)
    {
        var tableUI = GetTableUI();
        return Json(GetTableData(request, tableUI));
    }

    [HttpGet("add")]
    public virtual IActionResult Add()
    {
        var formUI = GetFormUI();
        ViewData["FormUI"] = formUI;
        ViewData["Title"] = _output.Trans($"Add {PageTitle}");
        return _page.Template("Admin/FormView");
    }

    [HttpPost("add")]
    public IActionResult AddFormSubmit(FormUIRequest request)
    {
        FormRequest = request;
        var formUI = GetFormUI();
        formUI.SetData(request.FormData);
        if (formUI.IsValid())
        {
            return (IActionResult)formUI.DoAction(request.SubmitAction)!;
        }

        TempData["MessageError"] = _output.Trans("Invalid request, please check again.");
        ViewData["FormUI"] = formUI;
        ViewData["Title"] = _output.Trans($"Add {PageTitle}");
        return _page.Template("Admin/FormView");
    }

    [HttpGet("edit/{id}")]
    public virtual IActionResult Edit(int id)
    {
        if (!ValidateQueryId(id))
        {
            return NotFound();
        }

        var formUI = GetFormUI(id);
        ViewData["FormUI"] = formUI;
        ViewData["Title"] = _output.Trans($"Edit {PageTitle}");
        return _page.Template("Admin/FormView");
    }

    [HttpPost("edit/{id}")]
    public virtual IActionResult EditFormSubmit(int id, FormUIRequest request)
    {
        if (!ValidateQueryId(id))
        {
            return NotFound();
        }

        FormRequest = request;
        var formUI = GetFormUI(id);
        formUI.SetData(request.FormData);
        if (formUI.IsValid())
        {
            return (IActionResult)formUI.DoAction(request.SubmitAction)!;
        }

        TempData["MessageError"] = _output.Trans("Invalid request, please check again.");
        ViewData["FormUI"] = formUI;
        ViewData["Title"] = _output.Trans($"Edit {PageTitle}");
        return _page.Template("Admin/FormView");
    }

    protected virtual TableUI GetTableUI()
    {
        return new TableUI();
    }

    protected virtual object GetTableData(TableUIRequest request, TableUI tableUI)
    {
        return new { };
    }

    protected virtual FormUI GetFormUI(int id = 0)
    {
        return new FormUI();
    }

    protected virtual bool ValidateQueryId(int id)
    {
        return true;
    }
}
