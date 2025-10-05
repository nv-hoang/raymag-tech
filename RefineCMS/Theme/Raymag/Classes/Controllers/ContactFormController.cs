using Microsoft.AspNetCore.Mvc;
using RefineCMS.Attributes;
using RefineCMS.Common;
using RefineCMS.Common.UI;
using RefineCMS.Controllers.Admin;
using RefineCMS.Models;

namespace RefineCMS.Theme.Raymag.Classes.Controllers;

[AdminController("contact-form")]
[Auth]
public class ContactFormController(AppFactory _appFactory) : PostTypeController(_appFactory)
{
    public override List<string> Hidden { get; set; } = ["Add", "AddFormSubmit", "EditFormSubmit"];
    protected override string PageTitle { get; set; } = "Contact Form";
    protected override string PluralPageTitle { get; set; } = "Contact Form";
    protected override string PostType { get; set; } = "contact-form";

    protected Dictionary<string, string> GetPageTemplates()
    {
        return new()
        {
            [""] = _output.Trans("All Pages"),
            ["home"] = _output.Trans("Home"),
        };
    }

    protected override TableUI DefineTable()
    {
        var _ = _output.Trans;
        var pageTemplate = GetPageTemplates();

        var table = new TableUI
        {
            Api = Url.Action("ApiList"),
            DownloadFiles = "Excel,CSV",
            Filters = [
                new TableFilter {
                    Name = "Meta_Info_PageTemplate",
                    Items = pageTemplate,
                },
            ],
            Columns = [
                new TableColumn {
                    Name = "PostTitle",
                    Title = _("Fullname"),
                    Width = 200,
                    Render = (row) => {
                        var url = Url.Action("Edit", new { id = row["Id"] });
                        return $@"<a href=""{url}"">{row["PostTitle"]}</a>";
                    }
                },
                new TableColumn {
                    Name = "Phone",
                    Title = _("Phone Number"),
                    Render = (row) => {
                        return _output.MetaValue("Info.Phone", row);
                    }
                },
                new TableColumn {
                    Name = "Company",
                    Title = _("Company"),
                    Render = (row) => {
                        return _output.MetaValue("Info.Company", row);
                    }
                },
                new TableColumn {
                    Name = "Email",
                    Title = _("E-mail"),
                    Render = (row) => {
                        return _output.MetaValue("Info.Email", row);
                    }
                },
                new TableColumn {
                    Name = "Page",
                    Title = _("Page"),
                    Render = (row) => {
                        if(pageTemplate.TryGetValue(row["Info.PageTemplate"], out var template)) {
                            return template;
                        }
                        return "";
                    }
                },
                new TableColumn {
                    Name = "CreatedAt",
                    Title = _("Date Created"),
                    Render = (row) => {
                        if(!string.IsNullOrEmpty(_output.MetaValue("Info.CreatedAt", row))) {
                            return row["Info.CreatedAt"];
                        }

                        return string.IsNullOrEmpty(row["CreatedAt"]) ? "" : DateTime.Parse(row["CreatedAt"]).ToString("yyyy-MM-dd (HH:mm)");
                    }
                },
            ],
        };

        return table;
    }

    protected override IQueryable<Post> DefineTableQuery()
    {
        return base.DefineTableQuery().OrderByDescending(x => x.CreatedAt);
    }

    protected override FormUI GetFormUI(int id = 0)
    {
        var form = new ContactForm(_output);
        form.BackUrl = Url.Action("Index");

        if (id != 0)
        {
            form.LoadModelData(PostInstance, _postMetaProvider);

            var pageTemplate = form.Find("Info.PageTemplate");
            if (pageTemplate != null)
            {
                var fieldValue = pageTemplate.GetValue<string>();

                // =====================================================
                var pages = GetPageTemplates();
                if (pages.TryGetValue(fieldValue!, out var template))
                {
                    pageTemplate.SetValue(template);
                }
            }
        }

        return form;
    }
}
