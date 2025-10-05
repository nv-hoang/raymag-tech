using Microsoft.EntityFrameworkCore;
using RefineCMS.Models;
using RefineCMS.Providers;
using RefineCMS.Requests;

namespace RefineCMS.Common.UI;

public class ButtonUI
{
    public virtual string Type { get; set; } = "primary";
    public virtual string Label { get; set; } = string.Empty;
    public virtual string? Link { get; set; } = string.Empty;
    public virtual string? Template { get; set; }
    public virtual Func<ButtonUI, string>? CustomRender { get; set; }

    public virtual string Render()
    {
        return CustomRender?.Invoke(this) ?? $@"<a-button type=""{Type}"" href=""{Link}"">{Label}</a-button>";
    }
}

public class TableColumn
{
    public virtual string Name { get; set; } = string.Empty;
    public virtual string Title { get; set; } = string.Empty;
    public virtual int Width { get; set; } = 0;
    public virtual Func<Dictionary<string, string>, string>? Render { get; set; }
    public virtual bool Hidden { get; set; } = false;

    public virtual string Output()
    {
        if (Hidden) return "";
        return $@"<ui-col name=""{Name}"" title=""{Title}"" :width=""{Width}"" ></ui-col>";
    }
}

public class TableFilter
{
    public virtual string Name { get; set; } = string.Empty;
    public virtual string Default { get; set; } = string.Empty;
    public virtual Dictionary<string, string> Items { get; set; } = [];
    public virtual Func<TableFilter, string>? Render { get; set; }

    public virtual string Output()
    {
        if (Render != null) return Render.Invoke(this);

        var content = $"\n" + string.Join($"\n", Items.Select(kv => $@"<a-radio value=""{kv.Key}"" alt=""{kv.Value.Trim('-').Trim(' ')}"">{kv.Value.Trim(' ')}</a-radio>")) + $"\n";
        return $@"<a-select v-model:value=""filter.{Name}"" show-search option-filter-prop=""alt"" style=""min-width: 150px;"">{content}</a-select>";
    }
}

public class TableUI
{
    public string Name { get; set; } = "list";
    public string? Api { get; set; } = "list";
    public string? DownloadFiles { get; set; } = "";
    public ButtonUI? AddNew { get; set; }
    public List<TableColumn> Columns { get; set; } = [];
    public List<TableFilter> Filters { get; set; } = [];

    public object Paginate<TQuery, TMeta>(TableUIRequest request, IQueryable<TQuery> query, ModelProvider<TMeta>? metaProvider = null, AppFactory? appFactory = null)
        where TQuery : BaseModel
        where TMeta : BaseModel
    {
        var MetaArrKey = "";
        List<string> MetaArr = [];
        foreach (var filter in request.filter)
        {
            if (!string.IsNullOrEmpty(filter.Value))
            {
                if (filter.Key.StartsWith("Post_"))
                {
                    var key = filter.Key.Replace("Post_", "");
                    query = query.Where(x => EF.Property<object>(x!, key)!.ToString()!.Equals(filter.Value));
                }
                else if (filter.Key.StartsWith("Meta_"))
                {
                    if (metaProvider != null)
                    {
                        var key = filter.Key.Replace("Meta_", "");
                        key = key.Replace("_", ".");
                        query = query
                                .Join(
                                    metaProvider.Query(),
                                    t1 => t1.Id,
                                    t2 => EF.Property<object>(t2!, "ObjectId"),
                                    (t1, t2) => new { T1 = t1, MetaKey = EF.Property<object>(t2!, "MetaKey"), MetaValue = EF.Property<object>(t2!, "MetaValue") }
                                )
                                .Where(x => x.MetaKey.ToString() == key && x.MetaValue.ToString() == filter.Value)
                                .Select(x => x.T1)
                                .Distinct();
                    }
                }
                else if (filter.Key.StartsWith("MetaArr_"))
                {
                    if (metaProvider != null)
                    {
                        if (string.IsNullOrEmpty(MetaArrKey))
                        {
                            var key = filter.Key.Replace("MetaArr_", "");

                            key = key.Substring(0, key.LastIndexOf("_"));
                            MetaArrKey = key.Replace("_", ".");
                        }

                        MetaArr.Add(filter.Value.ToString());
                    }
                }
                else if (filter.Key.StartsWith("Term_"))
                {
                    if (appFactory != null)
                    {
                        ModelProvider<TermRelationship> termRelationshipProvider = appFactory.Create<ModelProvider<TermRelationship>>();
                        ModelProvider<Term> termProvider = appFactory.Create<ModelProvider<Term>>();

                        var Ids = termProvider.Query().Where(x => x.ParentId == int.Parse(filter.Value)).Select(x => x.Id).Distinct().ToList();
                        Ids.Add(int.Parse(filter.Value));

                        query = query
                                .Join(
                                    termRelationshipProvider.Query(),
                                    t1 => t1.Id,
                                    t2 => t2.ObjectId,
                                    (t1, t2) => new { T1 = t1, t2.TermId }
                                )
                                .Where(x => Ids.Contains(x.TermId))
                                .Select(x => x.T1)
                                .Distinct();
                    }
                }
            }
        }

        if (!string.IsNullOrEmpty(MetaArrKey) && metaProvider != null)
        {
            var patterns = MetaArr.Select(f => $"%\"{f}\"%").ToList();
            query = query
                    .Join(
                        metaProvider.Query(),
                        t1 => t1.Id,
                        t2 => EF.Property<object>(t2!, "ObjectId"),
                        (t1, t2) => new { T1 = t1, MetaKey = (string)EF.Property<object>(t2!, "MetaKey"), MetaValue = (string)EF.Property<object>(t2!, "MetaValue") }
                    )
                    .Where(x => x.MetaKey == MetaArrKey && patterns.Any(pattern => EF.Functions.Like(x.MetaValue, pattern)))
                    .Select(x => x.T1)
                    .Distinct();
        }

        var total = query.Count();
        List<TQuery> list = [];
        if (request.Limit > 0)
        {
            list = query.Skip(request.Skip).Take(request.Limit).ToList();
        }
        else
        {
            list = query.ToList();
        }

        var dictList = list.Select(obj => obj!.GetType().GetProperties().ToDictionary(prop => prop.Name, prop => (prop.GetValue(obj)?.ToString() ?? ""))).ToList();

        var posts = new List<object>();
        foreach (var item in dictList)
        {
            if (metaProvider != null && item.ContainsKey("Id"))
            {
                var meta = metaProvider.GetMeta(int.Parse(item["Id"]!));
                foreach (var kvp in meta)
                {
                    item[kvp.Key] = kvp.Value;
                }
            }

            var row = new Dictionary<string, string>();
            row["Id"] = item["Id"];
            foreach (var col in Columns)
            {
                if (!col.Hidden)
                {
                    row[col.Name] = string.Empty;
                    if (col.Render != null)
                    {
                        row[col.Name] = col.Render.Invoke(item!);
                    }
                    else if (item.ContainsKey(col.Name))
                    {
                        row[col.Name] = item[col.Name]!;
                    }
                }
            }
            posts.Add(row);
        }

        return new { skip = request.Skip, limit = request.Limit, posts, total };
    }

    public object Paginate<T>(TableUIRequest request, IQueryable<T> query) where T : BaseModel
    {
        return Paginate<T, T>(request, query, null);
    }

    public TableColumn? FindColumn(string name)
    {
        return Columns.FirstOrDefault(c => c.Name == name);
    }

    public void HideColumns(List<string> columns)
    {
        foreach (var name in columns)
        {
            var column = FindColumn(name);
            if (column != null)
            {
                column.Hidden = true;
            }
        }
    }
}
