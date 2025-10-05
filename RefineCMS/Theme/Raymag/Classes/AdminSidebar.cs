using RefineCMS.Attributes;
using RefineCMS.Common;
using RefineCMS.Common.UI;

namespace RefineCMS.Theme.Raymag.Classes;

[Hook("Admin_Sidebar")]
public class AdminSidebar(Output _output) : IFilterHook<List<SidebarMenu>>
{
    public List<SidebarMenu> Apply(List<SidebarMenu> menus, params object[] args)
    {
        var sidebar = (Sidebar)args[0];

        var _ = _output.Trans;

        menus.InsertRange(2, [
            sidebar.CreateMenu(
                name: "manage_contact_form",
                label: "Contact Form",
                icon: "Forms",
                route: "ContactForm"
            ),
        ]);

        return menus;
    }
}
