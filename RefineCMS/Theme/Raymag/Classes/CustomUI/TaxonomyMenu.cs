using Newtonsoft.Json.Linq;
using RefineCMS.Attributes;
using RefineCMS.Common;
using RefineCMS.Common.UI;
using RefineCMS.Models;
using RefineCMS.Providers;

namespace RefineCMS.Theme.Raymag.Classes.CustomUI;

[Hook("Taxonomy_FormUI")]
public class TaxonomyMenu(Output _output, AppFactory _appFactory) : IFilterHook<FormUI>
{
    protected readonly ModelProvider<Term> _termProvider = _appFactory.Create<ModelProvider<Term>>();

    public FormUI Apply(FormUI form, params object[] args)
    {
        var (id, Taxonomy, FormData) = ((int)args[0], (TermTaxonomy)args[1], (JObject)args[2]);

        if (Taxonomy.PostType != "menu") return form;

        var _ = _output.Trans;
        var Languages = _output.GetLanguages();

        form.Find("Info")!.Childrens.InsertRange(1, [
            new Radio {
                Name = "MenuType",
                Label = _("Menu Type"),
                Items = {
                    { "link", _("Link") },
                    { "page", _("Page") },
                    { "post", _("Post") },
                },
                Default = "link"
            },
            new PostRelationshipSelect {
                Name = "PageId",
                Label = _("Page"),
                Factory = _appFactory,
                PostType = "page",
                ShowIf = "getVal(formdata, 'Info.MenuType') == 'page'"
            },
            new PostRelationshipSelect {
                Name = "PostId",
                Label = _("Post"),
                Factory = _appFactory,
                ShowIf = "getVal(formdata, 'Info.MenuType') == 'post'"
            },
        ]);

        form.Find("Info.Slug")!.Hidden = false;
        form.Find("Info.Slug")!.Label = _("URL");
        form.Find("Info.Slug")!.Required = false;
        form.Find("Info.Slug")!.ShowIf = "getVal(formdata, 'Info.MenuType') == 'link'";
        form.Find("Info.Slug")!.Validate = (field, _form) =>
        {
            //var url = Slug.Generate(field.GetValue<string>());
            //field.SetValue(url);
            if (_form.Find("Info.MenuType")!.GetValue<string>() != "link")
            {
                field.SetValue("");
                field.Hidden = true;
            }
            return true;
        };

        return form;
    }
}
