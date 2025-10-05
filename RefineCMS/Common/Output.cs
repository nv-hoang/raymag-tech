using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using RefineCMS.Helpers;
using RefineCMS.Models;
using RefineCMS.Providers;
using System.Text.RegularExpressions;

namespace RefineCMS.Common;

public class Output(IServiceProvider _serviceProvider)
{
    protected readonly CFG _cfg = _serviceProvider.GetRequiredService<CFG>();
    protected readonly Page _page = _serviceProvider.GetRequiredService<Page>();
    protected readonly HookProvider _hook = _serviceProvider.GetRequiredService<HookProvider>();
    protected readonly AppOptions _appOptions = _serviceProvider.GetRequiredService<AppOptions>();

    protected readonly ModelProvider<Post> _postProvider = _serviceProvider.GetRequiredService<ModelProvider<Post>>();
    protected readonly ModelProvider<PostMeta> _postMetaProvider = _serviceProvider.GetRequiredService<ModelProvider<PostMeta>>();

    protected readonly ModelProvider<Term> _termProvider = _serviceProvider.GetRequiredService<ModelProvider<Term>>();
    protected readonly ModelProvider<TermMeta> _termMetaProvider = _serviceProvider.GetRequiredService<ModelProvider<TermMeta>>();
    protected readonly ModelProvider<TermTaxonomy> _termTaxonomyProvider = _serviceProvider.GetRequiredService<ModelProvider<TermTaxonomy>>();
    protected readonly ModelProvider<TermRelationship> _termRelationshipProvider = _serviceProvider.GetRequiredService<ModelProvider<TermRelationship>>();

    public string AssetUrl(string url)
    {
        var version = _cfg.Version;
        if (_cfg.Debug)
        {
            version = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        }
        return $"{_cfg.WwwRoot}/dist/assets/{url}?v={version}";
    }

    public string ThemeAssetUrl(string url)
    {
        var version = _cfg.Version;
        if (_cfg.Debug)
        {
            version = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        }
        return $"{_cfg.WwwRoot}/theme/{_cfg.Theme}/dist/assets/{url}?v={version}";
    }

    public string ThemePath(string path)
    {
        return $"/Theme/{_cfg.Theme}/{path.TrimStart('/')}";
    }

    public string ThemeOption(string key, bool lang = false, string type = "theme-options")
    {
        var meta = _appOptions.GetOption(type);
        return MetaValue(key, meta, lang);
    }

    public Dictionary<string, Dictionary<string, string>> ThemeOptionArray(string key, string type = "theme-options")
    {
        var meta = _appOptions.GetOption(type);
        return MetaArray(key, meta);
    }

    public string PostField(string name)
    {
        if (_page.PostInstance == null) return "";
        return PropertyGetter.GetValue<string>(_page.PostInstance, name)!;
    }

    public string CustomField(string key, bool lang = false, bool toBr = false)
    {
        return MetaValue(key, _page.MetaPostInstance, lang, toBr);
    }

    public Dictionary<string, Dictionary<string, string>> CustomFieldArray(string key)
    {
        return MetaArray(key, _page.MetaPostInstance);
    }

    public string MetaValue(string keyName, Dictionary<string, string>? meta = null, bool lang = false, bool toBr = false)
    {
        if (meta != null)
        {
            if (lang)
            {
                if (meta!.TryGetValue($"{keyName}.{GetLanguageKey()}", out var langValue) && !string.IsNullOrEmpty(langValue))
                {
                    return toBr ? ToBr(langValue) : langValue;
                }
                else if (meta!.TryGetValue($"{keyName}.en", out var defaultLangValue))
                {
                    return toBr ? ToBr(defaultLangValue) : defaultLangValue;
                }
            }
            else if (meta!.TryGetValue(keyName, out var value))
            {
                return toBr ? ToBr(value) : value;
            }
        }

        return "";
    }

    public string ToBr(string? text)
    {
        return (text ?? "").Replace("\r\n", "<br>").Replace("\n", "<br>").Replace("\r", "<br>");
    }

    public string Inline(string? text)
    {
        return (text ?? "").Replace("\r", " ").Replace("\n", " ");
    }

    public Dictionary<string, Dictionary<string, string>> MetaArray(string keyName, Dictionary<string, string>? meta = null)
    {
        Dictionary<string, Dictionary<string, string>> rows = [];

        if (meta != null)
        {
            var items = meta.Where(x =>
            {
                string pattern = @"^" + Regex.Escape(keyName) + @"\.\d+\.Row\b";
                return Regex.IsMatch(x.Key, pattern);
            }).ToList();

            foreach (var item in items)
            {
                var keyId = item.Key.Substring(keyName.Length + 1);
                var segments = keyId.Split('.', 3);

                if (!rows.ContainsKey(segments[0])) rows[segments[0]] = [];
                rows[segments[0]][segments[2]] = item.Value;
            }
        }

        return rows;
    }

    public string Trans(string name)
    {
        //string filePath = Path.Combine("D:\\refinelab\\RefineCMS\\Resources", "lang.txt");
        //File.AppendAllText(filePath, name + Environment.NewLine);

        return $"{_page.Localizer?[name] ?? name}";
    }

    public string Logo(string type = "")
    {
        var url = AssetUrl("img/logo.svg");
        return _hook.ApplyFilters("Theme_Logo", url, type);
    }

    public string Url(string url, object? queryParams = null)
    {
        if (string.IsNullOrEmpty(url)) return "";
        if (url.StartsWith("http")) return url;

        url = _cfg.WwwRoot.TrimEnd('/') + '/' + url.TrimStart('/');
        if (queryParams != null)
        {
            var dictionary = queryParams.GetType()
                .GetProperties()
                .ToDictionary(
                    prop => prop.Name,
                    prop => prop.GetValue(queryParams)?.ToString() ?? string.Empty
                );
            return QueryHelpers.AddQueryString(url, dictionary!);
        }
        return url;
    }

    public T? GetDataValue<T>(object? data, string keys)
    {
        if (data == null || string.IsNullOrWhiteSpace(keys))
            return default;

        var segments = keys.Split('.', 2);
        var currentKey = segments[0];
        var remainingKeys = segments.Length > 1 ? segments[1] : null;

        object? next = null;

        if (data is IDictionary<string, object> dict)
        {
            if (!dict.TryGetValue(currentKey, out next))
                return default;
        }
        else if (data is Newtonsoft.Json.Linq.JObject jObject)
        {
            var token = jObject[currentKey];
            next = token?.Type == Newtonsoft.Json.Linq.JTokenType.Null ? null : token;
        }
        else
        {
            var type = data.GetType();
            var prop = type.GetProperty(currentKey);
            if (prop != null)
                next = prop.GetValue(data);
            else
                return default;
        }

        if (remainingKeys != null)
            return GetDataValue<T>(next, remainingKeys);

        try
        {
            if (next is T value)
                return value;

            if (next is Newtonsoft.Json.Linq.JValue jVal)
                return jVal.ToObject<T>();

            return (T)Convert.ChangeType(next!, typeof(T));
        }
        catch
        {
            return default;
        }
    }

    public List<KeyValuePair<string, string>> GetLanguages()
    {
        return _cfg.Languages.Select(x => new KeyValuePair<string, string>(x.Key, Trans($"lang_{x.Value}"))).ToList();
    }

    public string GetLanguage()
    {
        return Trans($"lang_{_page.Language}");
    }

    public string GetLanguageKey()
    {
        return _cfg.Languages.Where(x => x.Value == _page.Language).First().Key;
    }

    public List<Dictionary<string, object>> GetTermTree(string taxanomy, int parentId = 0, bool onlyPublish = true)
    {
        var _termProvider = _serviceProvider.GetRequiredService<ModelProvider<Term>>();
        var _termMetaProvider = _serviceProvider.GetRequiredService<ModelProvider<TermMeta>>();
        var _taxonomyProvider = _serviceProvider.GetRequiredService<ModelProvider<TermTaxonomy>>();

        var list = _termProvider.Query()
            .Join(_taxonomyProvider.Query(), t1 => t1.TaxonomyId, t2 => t2.Id, (t1, t2) => new { t1.Id, t1.Name, t1.Slug, t1.ParentId, t1.Order, t1.Status, TaxonomySlug = t2.Slug, TaxonomyStatus = t2.Status })
            .Where(x => x.TaxonomySlug.Equals(taxanomy) && (onlyPublish ? x.Status == 1 : true) && x.TaxonomyStatus == 1)
            .OrderBy(x => x.Order)
            .ToList();

        return ListToTree(list, parentId, _termMetaProvider);
    }

    protected List<Dictionary<string, object>> ListToTree<TQuery, TMeta>(List<TQuery> items, int parentId = 0, ModelProvider<TMeta>? _termMetaProvider = null)
        where TQuery : class
        where TMeta : BaseModel
    {
        var tree = new List<Dictionary<string, object>>();

        var children = items.Where(x => parentId.Equals(PropertyGetter.GetValue<int>(x, "ParentId")));

        foreach (var child in children)
        {
            var id = PropertyGetter.GetValue<int>(child, "Id");

            var ChildrenItems = ListToTree(items, id, _termMetaProvider);
            var treeItem = new Dictionary<string, object>
            {
                ["Id"] = id,
                ["Name"] = PropertyGetter.GetValue<string>(child, "Name") ?? "",
                ["Link"] = Url(PropertyGetter.GetValue<string>(child, "Slug") ?? ""),
                ["ParentId"] = PropertyGetter.GetValue<int>(child, "ParentId"),
                ["Children"] = ChildrenItems,
                ["HasChild"] = ChildrenItems.Count > 0
            };


            if (_termMetaProvider != null)
            {
                var meta = _termMetaProvider.GetMeta(id);
                treeItem["Meta"] = meta;

                if (true)
                {
                    treeItem["Name"] = MetaValue("Info.Name", meta, true);
                }
            }

            tree.Add(treeItem);
        }

        return tree;
    }

    public Term? GetTerm(int id)
    {
        return _termProvider.FindById(id);
    }

    public int GetTermId(string taxonomySlug, string metaKey, string metaValue, bool useLike = false)
    {
        return _termProvider.Query()
            .Join(_termMetaProvider.Query(), t1 => t1.Id, t2 => t2.ObjectId, (t1, t2) => new { t1.Id, t1.TaxonomyId, t1.Status, t2.MetaKey, t2.MetaValue })
            .Join(_termTaxonomyProvider.Query(), t1 => t1.TaxonomyId, t2 => t2.Id, (t1, t2) => new { t1.Id, t1.Status, t1.MetaKey, t1.MetaValue, TaxonomySlug = t2.Slug, TaxonomyStatus = t2.Status })
            .Where(x =>
                x.MetaKey.Equals(metaKey) &&
                (useLike ? EF.Functions.Like(x.MetaValue, $@"%""{metaValue}""%") : x.MetaValue.Equals(metaValue)) &&
                x.Status == 1 &&
                x.TaxonomyStatus == 1 &&
                x.TaxonomySlug == taxonomySlug
            )
            .Select(x => x.Id)
            .Distinct()
            .FirstOrDefault();
    }

    public Term? GetTermOfPost(int id, string taxonomy, bool onlyPublish = false)
    {
        return _termRelationshipProvider.Query()
            .Join(_termProvider.Query(), t1 => t1.TermId, t2 => t2.Id, (t1, t2) => new { t1.ObjectId, Term = t2 })
            .Join(_termTaxonomyProvider.Query(), t1 => t1.Term.TaxonomyId, t2 => t2.Id, (t1, t2) => new { t1.ObjectId, t1.Term, Taxonomy = t2 })
            .Where(x => x.ObjectId == id && x.Taxonomy.Slug == taxonomy && (onlyPublish ? (x.Term.Status == 1 && x.Taxonomy.Status == 1) : true))
            .Select(x => x.Term)
            .FirstOrDefault();
    }

    public string GetTermName(int postId, string taxonomy, bool onlyPublish = false, bool fullPath = false)
    {
        var term = GetTermOfPost(postId, taxonomy, onlyPublish);
        if (term != null)
        {
            if (fullPath && term.ParentId > 0)
            {
                var parent = GetTerm(term.ParentId);
                if (parent != null) return $@"{parent.Name} / {term.Name}";
            }

            return term.Name;
        }
        return "";
    }

    public Dictionary<string, string> GetTermMeta(int id)
    {
        return _termMetaProvider.GetMeta(id);
    }

    public List<Post> GetPostsOfTerm(string termId)
    {
        return _postProvider.Query()
                .Join(_termRelationshipProvider.Query(), t1 => t1.Id, t2 => t2.ObjectId, (t1, t2) => new { T1 = t1, t2.TermId })
                .Where(x => x.TermId.ToString() == termId && x.T1.PostStatus == "publish")
                .Select(x => x.T1)
                .ToList();
    }

    public Dictionary<string, string> GetPostMeta(int id)
    {
        return _postMetaProvider.GetMeta(id);
    }

    public Dictionary<string, string> GetTermFilter(string label, string taxonomy)
    {
        var categories = new Dictionary<string, string>();
        categories[""] = Trans($"All {label}");

        //foreach (var item in GetTermTree(taxonomy, 0, false))
        //{
        //    categories[item["Id"].ToString()!] = item["Name"].ToString()!;
        //}
        foreach (var item in BuildIndentedList(GetTermTree(taxonomy, 0, false)))
        {
            categories[item["Id"]] = item["Name"];
        }

        return categories;
    }

    public List<Dictionary<string, string>> BuildIndentedList(List<Dictionary<string, object>> items, int parentId = 0, int level = 0)
    {
        var result = new List<Dictionary<string, string>>();

        foreach (var item in items)
        {
            string indent = new string('-', level * 4);
            result.Add(new()
            {
                ["Id"] = item["Id"].ToString()!,
                ["Name"] = $"{indent} {item["Name"]}"
            });

            // recursively add children
            result.AddRange(BuildIndentedList((List<Dictionary<string, object>>)item["Children"], (int)item["Id"], level + 1));
        }

        return result;
    }

    public Dictionary<string, string> GetPage(string template)
    {
        Dictionary<string, string> meta = [];
        var page = _postProvider.FirstOrDefault(x => x.PostType == "page" && x.PostTemplate == template && x.PostStatus == "publish");
        if (page != null)
        {
            meta = _postMetaProvider.GetMeta(page.Id);

            meta["Id"] = page.Id.ToString();
            meta["Link"] = Url(page.PostName);
        }

        return meta;
    }

    public Dictionary<string, string> GetPostInfo(int id)
    {
        Dictionary<string, string> meta = [];
        var post = _postProvider.FirstOrDefault(x => x.Id == id && x.PostStatus == "publish");
        if (post != null)
        {
            meta = _postMetaProvider.GetMeta(post.Id);

            meta["Id"] = post.Id.ToString();
            meta["Link"] = Url(post.PostName);
        }

        return meta;
    }

    public string GetMenuLink(Dictionary<string, object> menu)
    {
        var link = menu["Link"].ToString();
        var meta = (Dictionary<string, string>)menu["Meta"];

        if (MetaValue("Info.MenuType", meta) == "page")
        {
            link = MetaValue("Link", GetPostInfo(int.Parse(MetaValue("Info.PageId", meta))));
        }
        else if (MetaValue("Info.MenuType", meta) == "post")
        {
            link = MetaValue("Link", GetPostInfo(int.Parse(MetaValue("Info.PostId", meta))));
        }
        return link ?? "";
    }
}
