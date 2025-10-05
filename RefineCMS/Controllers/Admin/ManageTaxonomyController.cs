using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefineCMS.Attributes;
using RefineCMS.Common;
using RefineCMS.Common.UI;
using RefineCMS.Helpers;
using RefineCMS.Models;
using RefineCMS.Providers;
using RefineCMS.Requests;

namespace RefineCMS.Controllers.Admin;

[AdminController("manage-taxonomy")]
[Auth]
public class ManageTaxonomyController(AppFactory _appFactory) : CRUDController(_appFactory)
{
    protected override string PageTitle { get; set; } = "Taxonomy Type";
    protected override string PluralPageTitle { get; set; } = "Taxonomy Types";
    protected readonly ModelProvider<TermTaxonomy> _provider = _appFactory.Create<ModelProvider<TermTaxonomy>>();
    protected TermTaxonomy? PostInstance;

    protected override TableUI GetTableUI()
    {
        var _ = _output.Trans;
        return new TableUI
        {
            Api = Url.Action("ApiList"),
            AddNew = new ButtonUI { Label = _($"Add New"), Link = Url.Action("Add") },
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
                        var url = Url.Action("Edit", new { id = row["Id"] });
                        return $@"<a href=""{url}"">{row["Name"]}</a>";
                    },
                },
                new TableColumn {
                    Name = "Slug",
                    Title = _("Slug"),
                },
                new TableColumn {
                    Name = "Order",
                    Title = _("Order"),
                },
                new TableColumn {
                    Name = "PostType",
                    Title = _("Post Type"),
                },
                new TableColumn {
                    Name = "Hierarchical",
                    Title = _("Hierarchical"),
                    Render = (row) => {
                        if("1".Equals(row["Hierarchical"])) {
                            return $@"<a-tag color=""green"">{ _("Yes")}</a-tag>";
                        }
                        return "";
                    }
                },
                new TableColumn {
                    Name = "Public",
                    Title = _("Public"),
                    Render = (row) => {
                        if("1".Equals(row["Public"])) {
                            return $@"<a-tag color=""green"">{ _("Yes")}</a-tag>";
                        }
                        return "";
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
    }

    protected override object GetTableData(TableUIRequest request, TableUI tableUI)
    {
        foreach (var id in request.DeleteIds)
        {
            DeletePost(id);
        }

        var query = _provider.Query();

        if (!string.IsNullOrEmpty(request.Search))
        {
            query = query.Where(x => EF.Functions.Like(x.Name, $"%{request.Search}%"));
        }

        return tableUI.Paginate(request, query);
    }

    protected override bool ValidateQueryId(int id)
    {
        PostInstance = _provider.FindById(id);
        if (PostInstance != null) return true;
        return false;
    }

    protected override FormUI GetFormUI(int id = 0)
    {
        var _ = _output.Trans;
        var isEdit = id != 0;

        var form = new FormUI
        {
            Fields = [
                new Group {
                    Name = "Info",
                    Label = "",
                    Childrens = [
                        new Text {
                            Name = "Name",
                            Label = _("Name"),
                            Required = true,
                        },
                        new Text {
                            Name = "PluralName",
                            Label = _("PluralName"),
                            Required = true,
                        },
                        new Text {
                            Name = "Slug",
                            Label = _("Slug"),
                            Required = true,
                            Validate = (field, form) => {
                                var url = Slug.Generate(field.GetValue<string>());

                                if(string.IsNullOrEmpty(url)) {
                                    field.Message = _("This field is required.");
                                    return false;
                                }

                                if(_provider.Exists(x => x.Id != id && x.Slug.Equals(url))) {
                                    field.Message = _("The Url already exists.");
                                    return false;
                                }

                                field.SetValue(url);

                                return true;
                            },
                        },
                        new Number {
                            Name = "Order",
                            Label = _("Order"),
                            Default = 0
                        },
                        new Text {
                            Name = "PostType",
                            Label = _("Post Type"),
                            Required = true,
                        },
                        new Radio {
                            Name = "Hierarchical",
                            Label = _("Hierarchical"),
                            Items = {
                                { "1", _("Enable") },
                                { "0", _("Disable") }
                            },
                            Default = "1",
                        },
                        new Radio {
                            Name = "Public",
                            Label = _("Public"),
                            Items = {
                                { "1", _("Enable") },
                                { "0", _("Disable") }
                            },
                            Default = "1",
                        },
                        new Radio {
                            Name = "Status",
                            Label = _("Status"),
                            Items = {
                                { "1", _("Enable") },
                                { "0", _("Disable") }
                            },
                            Default = "1",
                        },
                    ],
                    Meta = false,
                },
            ],
            BackUrl = Url.Action("Index"),
            Actions = [
                // Add
                new FormAction {
                    Hidden = isEdit,
                    Name = "add",
                    Label = _("Add New"),
                    OnAction = (form) => {
                        var post = AddPost(form);

                        _page.Controller.TempData["MessageSuccess"] = _("Added successfully!");
                        return _page.Controller.RedirectToAction("Edit", new { id = post.Id });
                    }
                },

                // Edit
                new FormAction {
                    Hidden = !isEdit,
                    Name = "save",
                    Label = _("Save Changes"),
                    OnAction = (form) => {
                        UpdatePost(id, form);

                        _page.Controller.TempData["MessageSuccess"] = _("Saved successfully!");
                        return _page.Controller.RedirectToAction("Edit", new { id });
                    }
                },
                new DeleteFormAction {
                    Hidden = !isEdit,
                    Label = _("Delete"),
                    OnAction = (form) => {
                        DeletePost(id);

                        _page.Controller.TempData["MessageSuccess"] = _("Deleted successfully!");
                        return _page.Controller.RedirectToAction("Index");
                    }
                },
            ]
        };

        if (isEdit)
        {
            form.LoadModelData<TermTaxonomy>(PostInstance);
        }

        return form;
    }

    protected TermTaxonomy AddPost(FormUI form)
    {
        var post = _provider.Add(new TermTaxonomy
        {
            Name = form.Find("Info.Name")?.GetValue<string>() ?? "",
            PluralName = form.Find("Info.PluralName")?.GetValue<string>() ?? "",
            Slug = form.Find("Info.Slug")?.GetValue<string>() ?? "",
            Order = form.Find("Info.Order")?.GetValue<int>() ?? 0,
            PostType = form.Find("Info.PostType")?.GetValue<string>() ?? "",
            Hierarchical = form.Find("Info.Hierarchical")?.GetValue<int>() ?? 0,
            Public = form.Find("Info.Public")?.GetValue<int>() ?? 0,
            Status = form.Find("Info.Status")?.GetValue<int>() ?? 0,
        });

        return post;
    }

    protected TermTaxonomy? UpdatePost(int id, FormUI form)
    {
        var post = _provider.FindById(id);
        if (post == null) return null;

        post.Name = form.Find("Info.Name")?.GetValue<string>() ?? "";
        post.PluralName = form.Find("Info.PluralName")?.GetValue<string>() ?? "";
        post.Slug = form.Find("Info.Slug")?.GetValue<string>() ?? "";
        post.Order = form.Find("Info.Order")?.GetValue<int>() ?? 0;
        post.PostType = form.Find("Info.PostType")?.GetValue<string>() ?? "";
        post.Hierarchical = form.Find("Info.Hierarchical")?.GetValue<int>() ?? 0;
        post.Public = form.Find("Info.Public")?.GetValue<int>() ?? 0;
        post.Status = form.Find("Info.Status")?.GetValue<int>() ?? 0;

        return _provider.Update(post);
    }

    protected void DeletePost(int id)
    {
        _provider.Delete(id);
    }
}
