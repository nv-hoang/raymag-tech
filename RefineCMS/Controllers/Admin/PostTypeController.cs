using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using RefineCMS.Common;
using RefineCMS.Common.UI;
using RefineCMS.Helpers;
using RefineCMS.Models;
using RefineCMS.Providers;
using RefineCMS.Requests;
using System.Text;

namespace RefineCMS.Controllers.Admin;
public class PostTypeController(AppFactory _appFactory) : CRUDController(_appFactory)
{
    protected readonly ModelProvider<Post> _postProvider = _appFactory.Create<ModelProvider<Post>>();
    protected readonly ModelProvider<PostMeta> _postMetaProvider = _appFactory.Create<ModelProvider<PostMeta>>();

    protected readonly ModelProvider<User> _userProvider = _appFactory.Create<ModelProvider<User>>();
    protected readonly ModelProvider<TermRelationship> _termRelationshipProvider = _appFactory.Create<ModelProvider<TermRelationship>>();
    protected readonly ModelProvider<Term> _termProvider = _appFactory.Create<ModelProvider<Term>>();
    protected readonly ModelProvider<TermMeta> _termMetaProvider = _appFactory.Create<ModelProvider<TermMeta>>();
    protected readonly ModelProvider<TermTaxonomy> _termTaxonomyProvider = _appFactory.Create<ModelProvider<TermTaxonomy>>();

    protected virtual string PostType { get; set; } = "post";
    protected virtual string PostTemplate { get; set; } = string.Empty;
    protected virtual Post? PostInstance { get; set; }

    [HttpGet("list-download/{filetype}")]
    public virtual IActionResult DownloadList(string filetype, TableUIRequest request)
    {
        List<string> supports = ["Excel", "CSV"];
        if (!supports.Contains(filetype)) return NotFound();

        var tableUI = GetTableUI();
        object data = GetTableData(request, tableUI);

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add(PageTitle);

            // Add header
            int i = 1;
            foreach (var col in tableUI.Columns)
            {
                if (!col.Hidden)
                {
                    worksheet.Cell(1, i).Value = col.Title;
                    i++;
                }
            }

            // Add data
            List<object> rows = PropertyGetter.GetValue<List<object>>(data, "posts")!;
            for (int j = 0; j < rows.Count; j++)
            {
                var row = rows[j] as Dictionary<string, string>;
                i = 1;
                foreach (var col in tableUI.Columns)
                {
                    if (!col.Hidden)
                    {
                        worksheet.Cell(j + 2, i).Value = System.Text.RegularExpressions.Regex.Replace(row![col.Name].ToString(), "<.*?>", string.Empty);
                        i++;
                    }
                }
            }

            if (filetype == "Excel")
            {
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"{PageTitle}.xlsx"
                    );
                }
            }
            else if (filetype == "CSV")
            {
                var csvData = new StringBuilder();
                int rowCount = worksheet.LastRowUsed()!.RowNumber();
                int colCount = worksheet.LastColumnUsed()!.ColumnNumber();

                for (int r = 1; r <= rowCount; r++)
                {
                    var rowValues = new List<string>();
                    for (int c = 1; c <= colCount; c++)
                    {
                        string cellValue = worksheet.Cell(r, c).GetValue<string>();
                        rowValues.Add(EscapeCsv(cellValue));
                    }
                    csvData.AppendLine(string.Join(",", rowValues));
                }

                var csvBytes = Encoding.UTF8.GetBytes(csvData.ToString());

                return File(
                    csvBytes,
                    "text/csv",
                    $"{PageTitle}.csv"
                );
            }
        }

        return NotFound();
    }


    private string EscapeCsv(string value)
    {
        if (string.IsNullOrEmpty(value))
            return "";

        bool mustQuote = value.Contains(",") || value.Contains("\"") || value.Contains("\n") || value.Contains("\r");
        if (mustQuote)
        {
            // Escape quotes by doubling them
            value = value.Replace("\"", "\"\"");
            return $"\"{value}\"";
        }
        return value;
    }


    protected virtual TableUI DefineTable()
    {
        var _ = _output.Trans;
        var table = new TableUI
        {
            Api = Url.Action("ApiList"),
            AddNew = new ButtonUI { Label = _($"Add New"), Link = Url.Action("Add") },
            Filters = [
                new TableFilter {
                    Name = "Post_PostStatus",
                    Items = {
                        { "", _("All Status") },
                        { "publish", _("Publish") },
                        { "draft", _("Draft") }
                    },
                },
            ],
            Columns = [
                new TableColumn {
                    Name = "PostTitle",
                    Title = _("Title"),
                    Render = (row) => {
                        var url = Url.Action("Edit", new { id = row["Id"] });
                        return $@"<a href=""{url}"">{row["PostTitle"]}</a>";
                    }
                },
                new TableColumn {
                    Name = "PostAuthor",
                    Title = _("Author"),
                    Render = (row) => {
                        var u = _userProvider.FindById(int.Parse(row["PostAuthor"]));
                        return u != null ? u.DisplayName : "";
                    }
                },
                new TableColumn {
                    Name = "Status",
                    Title = _("Status"),
                    Render = (row) => {
                        if("publish".Equals(row["PostStatus"])) {
                            return $@"<a-tag color=""green"">{ _("Publish")}</a-tag>";
                        }
                        if("draft".Equals(row["PostStatus"])) {
                            return $@"<a-tag>{ _("Draft")}</a-tag>";
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
                new TableColumn {
                    Name = "UpdatedAt",
                    Title = _("Last Modified"),
                    Render = (row) => {
                        return string.IsNullOrEmpty(row["UpdatedAt"]) ? "" : DateTime.Parse(row["UpdatedAt"]).ToString("yyyy-MM-dd (HH:mm)");
                    }
                },
            ],
        };

        return table;
    }

    protected override TableUI GetTableUI()
    {
        var table = DefineTable();
        table = _hook.ApplyFilters("PostType_TableUI", table, PostType, PostTemplate);

        return table;
    }

    protected virtual IQueryable<Post> DefineTableQuery()
    {
        return _postProvider.Query()
            .Where(x => x.PostType.Equals(PostType))
            .OrderByDescending(x => x.Pinned)
            .OrderByDescending(x => x.Order)
            .ThenByDescending(x => x.Id);
    }

    protected override object GetTableData(TableUIRequest request, TableUI tableUI)
    {
        foreach (var id in request.DeleteIds)
        {
            DeletePost(id);
        }

        var query = DefineTableQuery();

        if (!string.IsNullOrEmpty(request.Search))
        {
            query = query.Where(x => EF.Functions.Like(x.PostTitle, $"%{request.Search}%"));
        }

        query = _hook.ApplyFilters("PostType_TableQuery", query, request, PostType, PostTemplate);

        return tableUI.Paginate(request, query, _postMetaProvider, _appFactory);
    }

    protected override bool ValidateQueryId(int id)
    {
        PostInstance = _postProvider.FirstOrDefault(x => x.Id == id && x.PostType.Equals(PostType));
        if (PostInstance != null)
        {
            PostType = PostInstance.PostType;
            PostTemplate = PostInstance.PostTemplate;
            return true;
        }
        return false;
    }

    protected virtual FormUI DefineForm(int id = 0)
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
                            Name = "PostTitle",
                            Label = _("Title"),
                            Field = new Text { Required = true },
                            Languages = Languages
                        },

                        // Edit
                        new Text {
                            Name = "PostName",
                            Label = _("Slug"),
                            Required = isEdit,
                            Validate = (field, form) => {
                                var url = Slug.Generate(field.GetValue<string>());

                                if(field.Required && string.IsNullOrEmpty(url)) {
                                    field.Message = _("This field is required.");
                                    return false;
                                }

                                if(!string.IsNullOrEmpty(url) && _postProvider.Exists(x => x.Id != id && x.PostName.Equals(url))) {
                                    field.Message = _("The Url already exists.");
                                    return false;
                                }

                                field.SetValue(url);

                                return true;
                            },
                            Meta = false,
                        },

                        new LangGroup {
                            Name = "PostExcerpt",
                            Label = _("Excerpt"),
                            Field = new Textarea(),
                            Languages = Languages
                        },
                        new LangGroup {
                            Name = "PostContent",
                            Label = _("Content"),
                            Field = new Editor {  },
                            Languages = Languages
                        },
                        new UploadFile {
                            Name = "Thumbnail",
                            Label = _("Featured image"),
                            ImageOnly = true
                        },
                        new Radio {
                            Name = "PostStatus",
                            Label = _("Status"),
                            Items = {
                                { "publish", _("Publish") },
                                { "draft", _("Draft") }
                            },
                            Default = "draft",
                            Meta = false,
                        },
                    ],
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

        return form;
    }

    protected override FormUI GetFormUI(int id = 0)
    {
        var form = DefineForm(id);

        var info = form.Find("Info");
        if (form.Fields.Count > 1 && info != null && string.IsNullOrEmpty(info.Label))
        {
            info.Label = _output.Trans("General");
        }

        if (id != 0)
        {
            form = _hook.ApplyFilters("PostType_FormUI", form, id, PostType, PostTemplate, FormRequest == null ? new JObject() : FormRequest.GetData(), PostInstance!);
            form.LoadModelData(PostInstance, _postMetaProvider);
        }
        else
        {
            form = _hook.ApplyFilters("PostType_FormUI", form, id, PostType, PostTemplate, FormRequest == null ? new JObject() : FormRequest.GetData());
        }

        return form;
    }

    protected Post AddPost(FormUI form)
    {
        var title = form.Find("Info.PostTitle.en")?.GetValue<string>() ?? "";
        var postName = form.Find("Info.PostName")?.GetValue<string>() ?? "";

        if (form.Find("Info.PostName")?.Hidden == false && string.IsNullOrEmpty(postName))
        {
            postName = Slug.Generate(title);
            if (_postProvider.Exists(x => x.PostName.Equals(postName)))
            {
                postName += $"-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
            }
        }

        var date = DateTime.Now;
        var CreatedAt = form.Find("Info.CreatedAt");
        if (CreatedAt != null)
        {
            date = DateTime.Parse(CreatedAt.GetValue<string>()!);
        }

        var pinned = 0;
        if (form.Find("Info.Pinned") != null)
        {
            pinned = form.Find("Info.Pinned")?.GetValue<int>() ?? 0;
        }

        var post = _postProvider.Add(new Post
        {
            PostAuthor = _page.User!.Id,
            PostContent = form.Find("Info.PostContent.en")?.GetValue<string>() ?? "",
            PostTitle = title,
            PostExcerpt = form.Find("Info.PostExcerpt.en")?.GetValue<string>() ?? "",
            PostStatus = form.Find("Info.PostStatus")?.GetValue<string>() ?? "",
            PostName = postName,
            Pinned = pinned,
            PostType = PostType,
            PostTemplate = PostTemplate,
            CreatedAt = date,
        });

        form.SaveModelData(post.Id, _postMetaProvider);

        return post;
    }

    protected Post? UpdatePost(int id, FormUI form)
    {
        var post = _postProvider.FindById(id);
        if (post == null) return null;

        post.PostContent = form.Find("Info.PostContent.en")?.GetValue<string>() ?? "";
        post.PostTitle = form.Find("Info.PostTitle.en")?.GetValue<string>() ?? "";
        post.PostExcerpt = form.Find("Info.PostExcerpt.en")?.GetValue<string>() ?? "";
        post.PostStatus = form.Find("Info.PostStatus")?.GetValue<string>() ?? "";
        post.PostName = form.Find("Info.PostName")?.GetValue<string>() ?? "";

        if (form.Find("Info.Pinned") != null)
        {
            post.Pinned = form.Find("Info.Pinned")?.GetValue<int>() ?? 0;
        }

        var CreatedAt = form.Find("Info.CreatedAt");
        if (CreatedAt != null)
        {
            post.CreatedAt = DateTime.Parse(CreatedAt.GetValue<string>()!);
        }

        form.SaveModelData(id, _postMetaProvider);

        return _postProvider.Update(post);
    }

    protected void DeletePost(int id)
    {
        _postProvider.Delete(id);
        _postMetaProvider.DeleteMeta(id);
        _termRelationshipProvider.Delete(x => x.ObjectId == id);
    }
}