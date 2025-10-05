using RefineCMS.Attributes;
using RefineCMS.Common;
using RefineCMS.Common.UI;

namespace RefineCMS.Controllers.Admin;

[AdminController("posts")]
[Auth]
public class PostController(AppFactory _appFactory) : PostTypeController(_appFactory)
{
    protected override string PageTitle { get; set; } = "Post";
    protected override string PluralPageTitle { get; set; } = "Posts";

    protected override TableUI DefineTable()
    {
        var table = base.DefineTable();
        var _ = _output.Trans;

        table.Filters.Add(new TableFilter
        {
            Name = "Term_Category",
            Items = _output.GetTermFilter("Categories", "category"),
        });

        table.Columns.InsertRange(0, [
           new TableColumn {
                Name = "Thumbnail",
                Title = _("Image"),
                Render = (row) => {
                    var Thumbnail = _output.MetaValue("Info.Thumbnail", row);
                    if(string.IsNullOrEmpty(Thumbnail)) return "";
                    return @$"<img src=""{Thumbnail}"" style=""width: 80px;"">";
                }
            },
        ]);

        table.Columns.InsertRange(2, [
            new TableColumn {
                Name = "Category",
                Title = _("Category"),
                Render = (row) => _output.GetTermName(int.Parse(row["Id"]), "category")
            },
        ]);

        return table;
    }

    protected override FormUI DefineForm(int id = 0)
    {
        var form = base.DefineForm(id);
        var _ = _output.Trans;

        form.Find("Info")!.Childrens.InsertRange(1, [
            new Date {
                Name = "CreatedAt",
                Label = _("Date Created"),
                Default = DateTime.Now
            }
        ]);

        form.Find("Info")!.Childrens.InsertRange(4, [
            new TaxonomySelect {
                Name = "Category",
                Label = _("Category"),
                Taxonomy = "category",
                Factory = _appFactory
            }
        ]);

        return form;
    }
}
