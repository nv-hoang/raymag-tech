using RefineCMS.Attributes;
using RefineCMS.Common;

namespace RefineCMS.Theme.Raymag.Classes;

[Hook("Theme_Options")]
public class Options : IFilterHook<List<KeyValuePair<string, string>>>
{
    public List<KeyValuePair<string, string>> Apply(List<KeyValuePair<string, string>> list, params object[] args)
    {

        list.AddRange([
            new("theme-options", "Theme Options"),
            new("mail-smtp", "Mail SMTP"),
        ]);

        return list;
    }
}
