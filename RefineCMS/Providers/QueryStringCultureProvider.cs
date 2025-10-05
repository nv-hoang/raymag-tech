using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace RefineCMS.Providers;

public class QueryStringCultureProvider(Dictionary<string, string> _langMap) : RequestCultureProvider
{
    public override Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext context)
    {
        if (context.Request.Query.TryGetValue("lang", out var rawLang))
        {
            var lang = rawLang.ToString().Trim();
            if (_langMap.TryGetValue(lang, out var cultureCode))
            {
                var culture = new CultureInfo(cultureCode);
                var requestCulture = new RequestCulture(culture);

                // Set cookie for future requests
                context.Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(requestCulture),
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddYears(1),
                        IsEssential = true,
                        Path = "/"
                    }
                );

                return Task.FromResult<ProviderCultureResult?>(new ProviderCultureResult(cultureCode));
            }
        }

        return Task.FromResult<ProviderCultureResult?>(null);
    }
}
