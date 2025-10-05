using Microsoft.AspNetCore.Mvc;
using RefineCMS.Models;
using RefineCMS.Providers;

namespace RefineCMS.Common.UI;

public class SidebarMenu
{
    public virtual string Name { get; set; } = string.Empty;
    public virtual string Label { get; set; } = string.Empty;
    public virtual string Icon { get; set; } = string.Empty;
    public virtual bool Hidden { get; set; } = false;
    public virtual bool UseStrict { get; set; } = false;
    public virtual string? Link { get; set; }
    public virtual List<SidebarMenu> Childrens { get; set; } = [];
    public virtual Func<SidebarMenu, string, bool>? Active { get; set; }

    public bool IsActive(string contextPath)
    {
        if (Active != null) return Active.Invoke(this, contextPath);
        if (Childrens.Count > 0) return Childrens.Any(x => x.IsActive(contextPath));
        if (Link == null) return false;

        return UseStrict ? contextPath.Equals(Link) : contextPath.StartsWith(Link);
    }
}

public class Sidebar(IUrlHelper Url, AppFactory _appFactory)
{
    protected readonly CFG _cfg = _appFactory.Create<CFG>();
    protected readonly Output _output = _appFactory.Create<Output>();
    protected readonly HookProvider _hook = _appFactory.Create<HookProvider>();
    protected readonly ModelProvider<TermTaxonomy> _termTaxonomyProvider = _appFactory.Create<ModelProvider<TermTaxonomy>>();

    public List<SidebarMenu> GetTaxonomyMenu(string name)
    {
        return _termTaxonomyProvider.Query()
            .Where(x => (x.PostType == name || x.Slug == name) && x.Status == 1)
            .OrderBy(x => x.Order)
            .Select(x => new SidebarMenu
            {
                Name = x.Slug,
                Label = _output.Trans(x.PluralName),
                Link = Url.Action("TaxonomyIndex", "Taxonomy", new { taxonomy = x.Slug }),
            })
            .ToList();
    }

    public SidebarMenu CreateMenu(string name = "", string label = "", string? route = null, string icon = "Bookmark", bool useStrict = false, List<SidebarMenu>? childrens = null, Func<IUrlHelper, string?>? link = null)
    {
        if (link != null)
        {
            route = link(Url);
        }
        else if (route != null)
        {
            route = Url.Action("Index", route);
        }

        var menu = new SidebarMenu
        {
            Name = name,
            Label = _output.Trans(label),
            Icon = icon,
            Link = route,
            UseStrict = useStrict,
        };

        if (childrens != null)
        {
            menu.Childrens = childrens;
        }

        return menu;
    }

    public List<SidebarMenu> GetMenus()
    {
        var _ = _output.Trans;

        List<SidebarMenu> menus = [
            //CreateMenu(
            //    name: "manage_posts",
            //    label: "Posts",
            //    icon: "Posts",
            //    childrens: new[] {
            //        CreateMenu(name: "posts", label: "Posts", route: "Post")
            //    }
            //    .Concat(GetTaxonomyMenu("post"))
            //    .ToList()
            //),
            CreateMenu(
                name: "manage_page_and_menus",
                label: "Pages & Menus",
                icon: "Pages",
                childrens: new[] {
                    CreateMenu(name: "pages", label: "Pages", route: "Page")
                }
                .Concat(GetTaxonomyMenu("menu"))
                .Concat(GetTaxonomyMenu("page"))
                .ToList()
            ),
            CreateMenu(
                name: "manage_users",
                label: "Users",
                icon: "Users",
                childrens: new[] {
                    CreateMenu(name: "users", label: "Users", route: "User"),
                    CreateMenu(name: "user-roles", label: "Roles", route: "UserRole")
                }.ToList()
            ),
        ];

        // Add Options menu
        List<KeyValuePair<string, string>> list = [];
        list = _hook.ApplyFilters("Theme_Options", list);

        menus.Add(new SidebarMenu
        {
            Name = "manage_options",
            Label = _("Options"),
            Icon = "Options",
            Childrens = list.Select(x => new SidebarMenu { Label = _(x.Value), Link = Url.Action("Index", "Option", new { type = x.Key }) }).ToList()
        });

        if (_cfg.Debug)
        {
            menus.Add(CreateMenu(
                label: "Taxonomy Type",
                route: "ManageTaxonomy"
            ));
        }

        menus = _hook.ApplyFilters("Admin_Sidebar", menus, this);
        return menus;
    }

    public List<SidebarMenu> GetUserMenus(User? user)
    {
        var menus = GetMenus();
        List<string> permissions = [];

        if (user != null)
        {
            if (user.UserLogin == "admin") return menus;

            ModelProvider<Post> _postProvider = _appFactory.Create<ModelProvider<Post>>();
            ModelProvider<PostMeta> _postMetaProvider = _appFactory.Create<ModelProvider<PostMeta>>();
            ModelProvider<UserMeta> _userMetaProvider = _appFactory.Create<ModelProvider<UserMeta>>();

            var userMeta = _userMetaProvider.GetMeta(user.Id);
            var roleId = _output.MetaValue("Info.Role", userMeta);
            var role = _postProvider.Query().Where(x => x.Id.ToString() == roleId).FirstOrDefault();
            if (role != null)
            {
                var roleMeta = _postMetaProvider.GetMeta(role.Id);
                if (!string.IsNullOrEmpty(_output.MetaValue("Info.Permissions", roleMeta)))
                {
                    permissions = Newtonsoft.Json.Linq.JArray.Parse(_output.MetaValue("Info.Permissions", roleMeta)).Select(x => x.ToString()).ToList();
                }
            }
        }

        return menus.Where(x => permissions.Contains(x.Name)).ToList();
    }

    public List<string> GetUserUrls(User? user)
    {
        var menus = GetMenus();
        List<string> permissions = [];

        if (user != null)
        {
            ModelProvider<Post> _postProvider = _appFactory.Create<ModelProvider<Post>>();
            ModelProvider<PostMeta> _postMetaProvider = _appFactory.Create<ModelProvider<PostMeta>>();
            ModelProvider<UserMeta> _userMetaProvider = _appFactory.Create<ModelProvider<UserMeta>>();

            var userMeta = _userMetaProvider.GetMeta(user.Id);
            var roleId = _output.MetaValue("Info.Role", userMeta);
            var role = _postProvider.Query().Where(x => x.Id.ToString() == roleId).FirstOrDefault();
            if (role != null)
            {
                var roleMeta = _postMetaProvider.GetMeta(role.Id);
                if (!string.IsNullOrEmpty(_output.MetaValue("Info.Permissions", roleMeta)))
                {
                    permissions = Newtonsoft.Json.Linq.JArray.Parse(_output.MetaValue("Info.Permissions", roleMeta)).Select(x => x.ToString()).ToList();
                }
            }
        }

        List<string> urls = [];
        foreach (var item in menus.Where(x => permissions.Contains(x.Name)).ToList())
        {
            if (item.Childrens.Count == 0 && !string.IsNullOrEmpty(item.Link))
            {
                urls.Add(item.Link);
            }
            else
            {
                foreach (var child in item.Childrens)
                {
                    if (!string.IsNullOrEmpty(child.Link))
                    {
                        urls.Add(child.Link);
                    }
                }
            }
        }

        return urls;
    }
}
