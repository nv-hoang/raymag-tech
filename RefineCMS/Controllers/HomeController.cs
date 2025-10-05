using Microsoft.AspNetCore.Mvc;
using RefineCMS.Attributes;
using RefineCMS.Common;
using RefineCMS.Models;
using RefineCMS.Providers;

namespace RefineCMS.Controllers;

public class HomeController(AppFactory _appFactory, CFG _cfg) : BaseController(_appFactory)
{
    protected readonly ModelProvider<Post> _postProvider = _appFactory.Create<ModelProvider<Post>>();
    protected readonly ModelProvider<PostMeta> _postMetaProvider = _appFactory.Create<ModelProvider<PostMeta>>();
    protected readonly ModelProvider<TermTaxonomy> _termTaxonomyProvider = _appFactory.Create<ModelProvider<TermTaxonomy>>();
    protected readonly ModelProvider<Term> _termProvider = _appFactory.Create<ModelProvider<Term>>();
    protected readonly ModelProvider<TermMeta> _termMetaProvider = _appFactory.Create<ModelProvider<TermMeta>>();

    protected TermTaxonomy? Taxonomy { get; set; }

    [HttpGet("{*path}")]
    public IActionResult Handle(string path)
    {
        if (string.IsNullOrEmpty(path)) path = "/";
        path = path.Trim('/');

        // Admin handle
        if (path.StartsWith(_cfg.AdminSlug)) return NotFound();

        // Site handle
        var post = FindPath(path);

        if (post == null) return NotFound();

        _page.PostInstance = post;

        if (post is Post p)
        {
            _page.MetaPostInstance = _postMetaProvider.GetMeta(p.Id);
            return _page.ThemeTemplate(p.PostType, p.PostTemplate, p.Id);
        }

        if (post is Term term)
        {
            _page.MetaPostInstance = _termMetaProvider.GetMeta(term.Id);
            return _page.ThemeTemplate("Taxonomy", Taxonomy!.Slug, term.Id);
        }

        //return Content($"<a href='/admin'>path {path}</a>", "text/html");
        return NotFound();
    }

    protected BaseModel? FindPath(string path, int taxonomyId = 0, int parentId = 0)
    {
        var segments = path.Split('/', 2);
        var currentKey = segments[0];
        var remainingKeys = segments.Length > 1 ? segments[1] : null;

        if (taxonomyId == 0)
        {
            // Find Post
            var post = _postProvider.FirstOrDefault(x => (x.PostName.Equals(currentKey) || (x.PostType.Equals("page") && x.PostTemplate.Equals("home") && string.IsNullOrEmpty(path))) && x.PostStatus.Equals("publish"));
            if (post != null) return post;

            // Find Taxonomy
            Taxonomy = _termTaxonomyProvider.FirstOrDefault(x => x.Slug.Equals(currentKey) && x.Status == 1 && x.Public == 1);
            if (Taxonomy != null)
            {
                if (remainingKeys == null) return null;
                return FindPath(remainingKeys, Taxonomy.Id);
            }
        }
        else
        {
            // Find Term
            var term = _termProvider.FirstOrDefault(x => x.TaxonomyId == taxonomyId && x.Slug.Equals(currentKey) && x.Status == 1 && x.ParentId == parentId);
            if (term != null)
            {
                if (remainingKeys == null) return term;
                return FindPath(remainingKeys, taxonomyId, term.Id);
            }
        }

        return null;
    }

    [HttpGet("preview/{id}")]
    [Auth]
    public IActionResult Edit(int id)
    {
        List<string> pages = ["page", "post", "product"];
        var post = _postProvider.FirstOrDefault(x => x.Id == id && pages.Contains(x.PostType));
        if (post == null) return NotFound();

        _page.PostInstance = post;

        _page.MetaPostInstance = _postMetaProvider.GetMeta(post.Id);
        return _page.ThemeTemplate(post.PostType, post.PostTemplate, post.Id);
    }

    [HttpGet("error")]
    public IActionResult Error()
    {
        return _page.Template("Error");
    }
}
