using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefineCMS.Attributes;
using RefineCMS.Common;
using RefineCMS.Common.UI;
using RefineCMS.Models;
using RefineCMS.Providers;
using RefineCMS.Requests;

namespace RefineCMS.Controllers.Admin;

[AdminController("api")]
[Auth]
public class ApiController(AppFactory _appFactory) : Controller
{
    protected readonly Output _output = _appFactory.Create<Output>();

    protected readonly ModelProvider<Post> _postProvider = _appFactory.Create<ModelProvider<Post>>();
    protected readonly ModelProvider<PostMeta> _postMetaProvider = _appFactory.Create<ModelProvider<PostMeta>>();

    [HttpGet("search/{postType}")]
    public IActionResult SearchPostType(string postType, TableUIRequest request)
    {
        var table = new TableUI
        {
            Columns = [
                new TableColumn {
                    Name = "Title",
                    Render = (row) => {
                        var title = _output.MetaValue("PostTitle", row);
                        return $@"<a href=""{_output.Url(_output.MetaValue("PostName", row))}"" target=""_blank"">{title}</a>";
                    }
                },
            ]
        };

        var query = _postProvider.Query()
            .Where(x => x.PostType == postType);

        if (!string.IsNullOrEmpty(request.Search))
        {
            query = query.Where(x => EF.Functions.Like(x.PostTitle, $"%{request.Search}%"));
        }

        if (request.Ids.Count > 0)
        {
            query = query.Where(x => request.Ids.Contains(x.Id))
                    .AsEnumerable()
                    .OrderBy(x => request.Ids.IndexOf(x.Id))
                    .AsQueryable();
            request.Limit = 100;
        }
        else
        {
            query = query.OrderBy(x => x.PostTitle);
        }

        return Json(table.Paginate(request, query, _postMetaProvider, _appFactory));
    }
}
