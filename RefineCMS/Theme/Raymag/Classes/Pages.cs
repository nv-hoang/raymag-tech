using RefineCMS.Attributes;
using RefineCMS.Common;

namespace RefineCMS.Theme.Raymag.Classes;

[Hook("Theme_Pages")]
public class Pages : IFilterHook<Dictionary<string, string>>
{
    public Dictionary<string, string> Apply(Dictionary<string, string> list, params object[] args)
    {
        list = new Dictionary<string, string>
        {
            ["home"] = "01. Home",
        };
        return list;
    }
}
