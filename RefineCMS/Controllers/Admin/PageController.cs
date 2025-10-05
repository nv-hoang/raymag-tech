using Microsoft.AspNetCore.Mvc;
using RefineCMS.Attributes;
using RefineCMS.Common;
using RefineCMS.Common.UI;
using RefineCMS.Requests;

namespace RefineCMS.Controllers.Admin;

[AdminController("pages")]
[Auth]
public class PageController(AppFactory _appFactory) : PostTypeController(_appFactory)
{
    public override List<string> Hidden { get; set; } = ["Add", "AddFormSubmit"];
    protected override string PageTitle { get; set; } = "Page";
    protected override string PluralPageTitle { get; set; } = "Pages";
    protected override string PostType { get; set; } = "page";

    [HttpGet("add/{template}")]
    public virtual IActionResult PageAdd(string template)
    {
        if (!ValidatePageTemplate(template)) return NotFound();
        return base.Add();
    }

    [HttpPost("add/{template}")]
    public IActionResult PageAddFormSubmit(string template, FormUIRequest request)
    {
        if (!ValidatePageTemplate(template)) return NotFound();
        return base.AddFormSubmit(request);
    }

    protected bool ValidatePageTemplate(string template)
    {
        Dictionary<string, string> list = [];
        list = _hook.ApplyFilters("Theme_Pages", list);
        if (list.ContainsKey(template))
        {
            PostTemplate = template;
            return true;
        }
        return false;
    }

    protected override TableUI DefineTable()
    {
        var _ = _output.Trans;
        var table = base.DefineTable();

        Dictionary<string, string> pagePist = [];
        pagePist = _hook.ApplyFilters("Theme_Pages", pagePist);

        table.AddNew = new ButtonUI { Label = _($"Add {PageTitle}"), Template = "NewPageModal.cshtml" };
        table.Columns.InsertRange(1, [
            new TableColumn {
                Name = "PostName",
                Title = _("Slug"),
                Render = (row) => {
                    if(row["PostStatus"] == "publish") {
                        return $@"<a href=""{_output.Url(row["PostName"])}"" target=""_blank"">{row["PostName"]}</a>";
                    }
                    return row["PostName"];
                }
            },
            new TableColumn {
                Name = "PostTemplate",
                Title = _("Template"),
                Render = (row) => {
                    return pagePist.TryGetValue(row["PostTemplate"], out var value) ? value : "";
                }
            },
         ]);

        return table;
    }

    protected override FormUI DefineForm(int id = 0)
    {
        var form = base.DefineForm(id);

        form.HideFields(["Info.PostContent"]);

        form.Find("Info")!.Childrens.InsertRange(0, [
            new Text {
                Name = "PostTemplate",
                Label = _output.Trans("Template"),
                Value = PostTemplate,
                Disabled = true
            }
        ]);

        return form;
    }
}
