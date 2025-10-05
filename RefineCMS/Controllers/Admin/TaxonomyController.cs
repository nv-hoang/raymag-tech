using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using RefineCMS.Attributes;
using RefineCMS.Common;
using RefineCMS.Common.UI;
using RefineCMS.Helpers;
using RefineCMS.Models;
using RefineCMS.Providers;
using RefineCMS.Requests;

namespace RefineCMS.Controllers.Admin;

[AdminController("taxonomy")]
[Auth]
public class TaxonomyController(AppFactory _appFactory) : CRUDController(_appFactory)
{
    public override List<string> Hidden { get; set; } = ["Index", "ApiList", "Add", "AddFormSubmit", "Edit", "EditFormSubmit"];

    protected readonly ModelProvider<TermTaxonomy> _taxonomyProvider = _appFactory.Create<ModelProvider<TermTaxonomy>>();
    protected readonly ModelProvider<Term> _termProvider = _appFactory.Create<ModelProvider<Term>>();
    protected readonly ModelProvider<TermMeta> _termMetaProvider = _appFactory.Create<ModelProvider<TermMeta>>();
    protected readonly ModelProvider<TermRelationship> _termRelationshipProvider = _appFactory.Create<ModelProvider<TermRelationship>>();
    protected readonly ModelProvider<Post> _postProvider = _appFactory.Create<ModelProvider<Post>>();

    protected TermTaxonomy Taxonomy { get; set; } = new TermTaxonomy();
    protected virtual Term? TermInstance { get; set; }

    protected bool ValidateTaxonomy(string taxonomy)
    {
        var result = _taxonomyProvider.FirstOrDefault(x => x.Slug.Equals(taxonomy) && x.Status == 1);
        if (result == null) return false;
        Taxonomy = result;
        PageTitle = Taxonomy.Name;
        PluralPageTitle = Taxonomy.PluralName;
        return true;
    }

    [HttpGet("{taxonomy}")]
    public virtual IActionResult TaxonomyIndex(string taxonomy)
    {
        if (!ValidateTaxonomy(taxonomy)) return NotFound();

        ViewData["Title"] = _output.Trans(PluralPageTitle);

        if (Taxonomy.Hierarchical == 1)
        {
            var treeUI = GetTreeUI();
            ViewData["TreeUI"] = treeUI;
            return _page.Template("Admin/TreeView");
        }

        return base.Index();
    }

    [HttpPost("{taxonomy}")]
    public IActionResult TaxonomyIndexSubmit(string taxonomy, FormUIRequest request)
    {
        if (!ValidateTaxonomy(taxonomy)) return NotFound();

        var treeUI = GetTreeUI();
        treeUI.SetData(request.FormData);
        return (IActionResult)treeUI.DoAction(request.SubmitAction)!;
    }

    [HttpGet("{taxonomy}/list")]
    public virtual IActionResult TaxonomyApiList(string taxonomy, TableUIRequest request)
    {
        if (!ValidateTaxonomy(taxonomy)) return NotFound();
        return base.ApiList(request);
    }

    [HttpGet("{taxonomy}/add")]
    public virtual IActionResult TaxonomyAdd(string taxonomy)
    {
        if (!ValidateTaxonomy(taxonomy)) return NotFound();

        return base.Add();
    }

    [HttpPost("{taxonomy}/add")]
    public IActionResult TaxonomyAddFormSubmit(string taxonomy, FormUIRequest request)
    {
        if (!ValidateTaxonomy(taxonomy)) return NotFound();

        return base.AddFormSubmit(request);
    }

    [HttpGet("{taxonomy}/edit/{id}")]
    public virtual IActionResult TaxonomyEdit(string taxonomy, int id)
    {
        if (!ValidateTaxonomy(taxonomy)) return NotFound();

        return base.Edit(id);
    }

    [HttpPost("{taxonomy}/edit/{id}")]
    public virtual IActionResult TaxonomyEditFormSubmit(string taxonomy, int id, FormUIRequest request)
    {
        if (!ValidateTaxonomy(taxonomy)) return NotFound();

        return base.EditFormSubmit(id, request);
    }

    protected override TableUI GetTableUI()
    {
        var _ = _output.Trans;
        var table = new TableUI
        {
            Api = Url.Action("TaxonomyApiList", new { taxonomy = Taxonomy.Slug }),
            AddNew = new ButtonUI { Label = _($"Add {PageTitle}"), Link = Url.Action("TaxonomyAdd", new { taxonomy = Taxonomy.Slug }) },
            Filters = [
                new TableFilter {
                    Name = "Post_Status",
                    Items = {
                        { "", _("All Status") },
                        { "1", _("Enable") },
                        { "0", _("Disable") }
                    },
                },
            ],
            Columns = [
                new TableColumn {
                    Name = "Name",
                    Title = _("Name"),
                    Render = (row) => {
                        var url = Url.Action("TaxonomyEdit", new { taxonomy = Taxonomy.Slug, id = row["Id"] });
                        return $@"<a href=""{url}"">{row["Name"]}</a>";
                    }
                },
                new TableColumn {
                    Name = "Slug",
                    Title = _("Slug"),
                    Hidden = Taxonomy.Public == 0
                },
                new TableColumn {
                    Name = "Description",
                    Title = _("Description")
                },
                new TableColumn {
                    Name = "Count",
                    Title = _("Count"),
                    Width = 100,
                    Render = (row) => {
                        return _termRelationshipProvider.Query()
                                .GroupBy(x => x.TermId)
                                .Where(x => x.Key == int.Parse(row["Id"]))
                                .Select(x => x.Count())
                                .FirstOrDefault()
                                .ToString();
                    }
                },
                new TableColumn {
                    Name = "Status",
                    Title = _("Status"),
                    Render = (row) => {
                        return "1".Equals(row["Status"]) ? $@"<a-tag color=""green"">{ _("Enable")}</a-tag>":$@"<a-tag color=""red"">{_("Disable")}</a-tag>";
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
                new TableColumn {
                    Name = "UpdatedAt",
                    Title = _("Last Modified"),
                    Render = (row) => {
                        return string.IsNullOrEmpty(row["UpdatedAt"]) ? "" : DateTime.Parse(row["UpdatedAt"]).ToString("yyyy-MM-dd (HH:mm)");
                    }
                },
            ],
        };

        table = _hook.ApplyFilters("Taxonomy_TableUI", table, Taxonomy);

        return table;
    }

    protected TreeUI GetTreeUI()
    {
        var _ = _output.Trans;

        var tree = new TreeUI
        {
            AddNew = new ButtonUI { Label = _($"Add {PageTitle}"), Link = Url.Action("TaxonomyAdd", new { taxonomy = Taxonomy.Slug }) },
            EditLink = Url.Action("TaxonomyEdit", new { taxonomy = Taxonomy.Slug, id = "_id_" }),
            Actions = [
                new TreeAction {
                    Type = "default",
                    Name = "save",
                    Label = _("Save Changes"),
                    OnAction = (form) => {
                        form.SaveModelData(_termProvider);

                        _page.Controller.TempData["MessageSuccess"] = _("Saved successfully!");
                        return _page.Controller.RedirectToAction("TaxonomyIndex", new { taxonomy = Taxonomy.Slug });
                    }
                },
            ],
        };

        var list = _termProvider.Query()
            .Join(_taxonomyProvider.Query(), t1 => t1.TaxonomyId, t2 => t2.Id, (t1, t2) => new { t1.Id, t1.Name, t1.ParentId, t1.Order, t1.Status, t2.Slug })
            .Where(x => x.Slug.Equals(Taxonomy.Slug))
            .OrderBy(x => x.Order)
            .ToList();

        tree.LoadModelData(list, _termMetaProvider);

        return tree;
    }

    protected override object GetTableData(TableUIRequest request, TableUI tableUI)
    {
        foreach (var id in request.DeleteIds)
        {
            DeleteTerm(id);
        }

        var query = _termProvider.Query().Where(x => x.TaxonomyId == Taxonomy.Id);

        if (!string.IsNullOrEmpty(request.Search))
        {
            query = query.Where(x => EF.Functions.Like(x.Name, $"%{request.Search}%"));
        }

        return tableUI.Paginate(request, query, _termMetaProvider);
    }

    protected override bool ValidateQueryId(int id)
    {
        TermInstance = _termProvider.FindById(id);
        return TermInstance != null;
    }

    protected override FormUI GetFormUI(int id = 0)
    {
        var _ = _output.Trans;
        var Languages = _output.GetLanguages();

        var isEdit = id != 0;

        var form = new FormUI
        {
            Fields = [
                new Group {
                    Name = "Info",
                    Label = "",
                    Childrens = [
                        new LangGroup {
                            Name = "Name",
                            Label = _("Name"),
                            Field = new Text { Required = true },
                            Languages = Languages
                        },
                        new Text {
                            Name = "Slug",
                            Label = _("Slug"),
                            Required = isEdit,
                            Hidden = Taxonomy.Public == 0,
                            Validate = (field, form) => {
                                var url = Slug.Generate(field.GetValue<string>());

                                if(field.Required && string.IsNullOrEmpty(url)) {
                                    field.Message = _("This field is required.");
                                    return false;
                                }

                                if(!string.IsNullOrEmpty(url) && _termProvider.Exists(x => x.Id != id && x.Slug.Equals(url))) {
                                    field.Message = _("The Url already exists.");
                                    return false;
                                }

                                field.SetValue(url);

                                return true;
                            },
                            Meta = false,
                        },
                        new Textarea {
                            Name = "Description",
                            Label = _("Description"),
                            Meta = false
                        },
                        new TaxonomyRelationshipSelect {
                            Hidden = Taxonomy.Hierarchical == 0,
                            Name = "ParentId",
                            Label = _("Parent"),
                            Taxonomy = Taxonomy.Slug,
                            Factory = _appFactory,
                            Meta = false,
                            EditId = id,
                            OnlyRoot = true
                        },
                        new Radio {
                            Name = "Status",
                            Label = _("Status"),
                            Items = {
                                { "1", _("Enable") },
                                { "0", _("Disable") }
                            },
                            Default = "0",
                            Meta = false,
                        },
                    ],
                },
            ],
            BackUrl = Url.Action("TaxonomyIndex", new { taxonomy = Taxonomy.Slug }),
            Actions = [
                // Add
                new FormAction {
                    Hidden = isEdit,
                    Name = "add",
                    Label = _("Add New"),
                    OnAction = (form) => {
                        var term = AddTerm(form);

                        _page.Controller.TempData["MessageSuccess"] = _("Added successfully!");
                        return _page.Controller.RedirectToAction("TaxonomyEdit", new { taxonomy = Taxonomy.Slug, id = term.Id });
                    }
                },

                // Edit
                new FormAction {
                    Hidden = !isEdit,
                    Name = "save",
                    Label = _("Save Changes"),
                    OnAction = (form) => {
                        UpdateTerm(id, form);

                        _page.Controller.TempData["MessageSuccess"] = _("Saved successfully!");
                        return _page.Controller.RedirectToAction("TaxonomyEdit", new { taxonomy = Taxonomy.Slug, id });
                    }
                },
                new DeleteFormAction {
                    Hidden = !isEdit,
                    Label = _("Delete"),
                    OnAction = (form) => {
                        DeleteTerm(id);

                        _page.Controller.TempData["MessageSuccess"] = _("Deleted successfully!");
                        return _page.Controller.RedirectToAction("TaxonomyIndex", new { taxonomy = Taxonomy.Slug });
                    }
                },
            ]
        };


        if (isEdit)
        {
            form = _hook.ApplyFilters("Taxonomy_FormUI", form, id, Taxonomy, FormRequest == null ? new JObject() : FormRequest.GetData(), TermInstance!);
            form.LoadModelData(TermInstance, _termMetaProvider);
        }
        else
        {
            form = _hook.ApplyFilters("Taxonomy_FormUI", form, id, Taxonomy, FormRequest == null ? new JObject() : FormRequest.GetData());
        }

        return form;
    }

    protected Term AddTerm(FormUI form)
    {
        var title = form.Find("Info.Name.en")?.GetValue<string>() ?? "";
        var url = form.Find("Info.Slug")?.GetValue<string>() ?? "";

        if (form.Find("Info.Slug")?.Hidden == false && string.IsNullOrEmpty(url))
        {
            url = Slug.Generate(title);
            if (_termProvider.Exists(x => x.Slug.Equals(url)))
            {
                url += $"-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
            }
        }

        var term = _termProvider.Add(new Term
        {
            Name = title,
            Slug = url,
            Description = form.Find("Info.Description")?.GetValue<string>() ?? "",
            Status = form.Find("Info.Status")?.GetValue<int>() ?? 0,
            TaxonomyId = Taxonomy.Id,
            ParentId = form.Find("Info.ParentId")?.GetValue<int>() ?? 0,
        });

        form.SaveModelData(term.Id, _termMetaProvider);

        return term;
    }

    protected Term? UpdateTerm(int id, FormUI form)
    {
        var term = _termProvider.FindById(id);
        if (term == null) return null;

        term.Name = form.Find("Info.Name.en")?.GetValue<string>() ?? "";
        term.Slug = form.Find("Info.Slug")?.GetValue<string>() ?? "";
        term.Description = form.Find("Info.Description")?.GetValue<string>() ?? "";
        term.Status = form.Find("Info.Status")?.GetValue<int>() ?? 0;
        term.ParentId = form.Find("Info.ParentId")?.GetValue<int>() ?? 0;

        form.SaveModelData(id, _termMetaProvider);

        return _termProvider.Update(term);
    }

    protected void DeleteTerm(int id)
    {
        _termProvider.Delete(id);
        _termMetaProvider.DeleteMeta(id);
        _termRelationshipProvider.Delete(x => x.TermId == id);
    }
}
