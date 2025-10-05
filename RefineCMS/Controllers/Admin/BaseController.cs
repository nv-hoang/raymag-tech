using Microsoft.AspNetCore.Mvc;
using RefineCMS.Common;

namespace RefineCMS.Controllers.Admin;

public class BaseController(AppFactory _appFactory) : Controller
{
    protected readonly Page _page = _appFactory.Create<Page>();
    protected readonly Output _output = _appFactory.Create<Output>();
    protected readonly HookProvider _hook = _appFactory.Create<HookProvider>();

    protected virtual string PageTitle { get; set; } = string.Empty;
    protected virtual string PluralPageTitle { get; set; } = string.Empty;
    public virtual List<string> Hidden { get; set; } = [];
}
