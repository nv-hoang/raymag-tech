using Slugify;

namespace RefineCMS.Helpers;

public static class Slug
{
    public static string Generate(string? title = null)
    {
        if (string.IsNullOrEmpty(title)) return "";

        var slugHelper = new SlugHelper();
        return slugHelper.GenerateSlug(title.Trim('/'));
    }

    public static string ToCamelCase(string slug, bool lowerFirst = false)
    {
        var parts = slug.Split(['-', '_'], StringSplitOptions.RemoveEmptyEntries);
        var result = string.Concat(parts.Select(p => char.ToUpper(p[0]) + p.Substring(1)));

        return lowerFirst && result.Length > 0
            ? char.ToLower(result[0]) + result.Substring(1)
            : result;
    }
}
