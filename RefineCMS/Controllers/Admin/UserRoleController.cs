using RefineCMS.Attributes;
using RefineCMS.Common;
using RefineCMS.Common.UI;

namespace RefineCMS.Controllers.Admin;

[AdminController("user-roles")]
[Auth]
public class UserRoleController(AppFactory _appFactory) : PostTypeController(_appFactory)
{
    protected override string PageTitle { get; set; } = "User Role";
    protected override string PluralPageTitle { get; set; } = "Roles";
    protected override string PostType { get; set; } = "user-role";

    protected override TableUI DefineTable()
    {
        var _ = _output.Trans;
        var table = base.DefineTable();

        table.FindColumn("PostTitle")!.Title = _("Name");
        table.HideColumns(["PostAuthor", "Status"]);

        return table;
    }

    protected override FormUI DefineForm(int id = 0)
    {
        var _ = _output.Trans;
        var Languages = _output.GetLanguages();
        var isEdit = id != 0;
        var form = base.DefineForm(id);

        form.HideFields([
            "Info.PostName",
            "Info.PostExcerpt",
            "Info.PostContent",
            "Info.Thumbnail",
            "Info.PostStatus"
        ]);

        form.Find("Info.PostTitle")!.Label = _("Name");
        form.Find("Info.PostTitle")!.HideLanguages(Languages);

        List<SidebarMenu> menus = (new Sidebar(Url, _appFactory)).GetMenus().Where(x => !string.IsNullOrEmpty(x.Name)).ToList();

        form.Find("Info")!.Childrens.InsertRange(2, [
           new Checkbox {
                Name = "Permissions",
                Label = _("Permissions"),
                Cols = 1,
                Items = menus.ToDictionary(x => x.Name, x => x.Label)
            }
        ]);

        return form;
    }
}
