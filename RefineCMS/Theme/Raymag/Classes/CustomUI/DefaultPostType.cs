using Newtonsoft.Json.Linq;
using RefineCMS.Attributes;
using RefineCMS.Common;
using RefineCMS.Common.UI;
using RefineCMS.Models;

namespace RefineCMS.Theme.Raymag.Classes.CustomUI;

[Hook("PostType_FormUI")]
public class DefaultPostType(Output _output) : IFilterHook<FormUI>
{
    public FormUI Apply(FormUI form, params object[] args)
    {
        var (id, postType, postTemplate, formData) = ((int)args[0], (string)args[1], (string)args[2], (JObject)args[3]);

        // Add View button on Edit
        if (id != 0 && new[] { "page", "post" }.Contains(postType))
        {
            var post = (Post)args[4];

            if (post.PostStatus == "publish")
            {
                var url = post.PostName;
                if (post.PostType == "page" && post.PostTemplate == "home")
                {
                    url = "/";
                }

                if (!string.IsNullOrEmpty(url))
                {
                    form.Actions.Insert(0, new ButtonFormAction
                    {
                        Label = _output.Trans("View"),
                        Link = _output.Url(url)
                    });
                }
            }
            else
            {
                form.Actions.Insert(0, new ButtonFormAction
                {
                    Label = _output.Trans("View"),
                    Link = _output.Url($"/preview/{post.Id}")
                });
            }
        }

        return form;
    }
}
