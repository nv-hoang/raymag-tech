using Microsoft.AspNetCore.Mvc;
using RefineCMS.Common;

namespace RefineCMS.Controllers;

public class BaseController(AppFactory _appFactory) : Controller
{
    protected readonly Page _page = _appFactory.Create<Page>();
    protected readonly Output _output = _appFactory.Create<Output>();
    protected readonly HookProvider _hook = _appFactory.Create<HookProvider>();
}

public class EmptyController : Controller
{ }

