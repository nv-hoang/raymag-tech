using Microsoft.AspNetCore.Mvc;
using RefineCMS.Common;
using RefineCMS.Controllers;
using RefineCMS.Models;
using RefineCMS.Providers;
using RefineCMS.Requests;

namespace RefineCMS.Theme.Raymag.Classes.Controllers;

[Route("api")]
public class ApiController(AppFactory _appFactory) : BaseController(_appFactory)
{
    protected readonly ModelProvider<Post> _postProvider = _appFactory.Create<ModelProvider<Post>>();
    protected readonly ModelProvider<PostMeta> _postMetaProvider = _appFactory.Create<ModelProvider<PostMeta>>();

    protected readonly ModelProvider<User> _userProvider = _appFactory.Create<ModelProvider<User>>();
    protected readonly ModelProvider<TermRelationship> _termRelationshipProvider = _appFactory.Create<ModelProvider<TermRelationship>>();
    protected readonly ModelProvider<Term> _termProvider = _appFactory.Create<ModelProvider<Term>>();
    protected readonly ModelProvider<TermMeta> _termMetaProvider = _appFactory.Create<ModelProvider<TermMeta>>();
    protected readonly ModelProvider<TermTaxonomy> _termTaxonomyProvider = _appFactory.Create<ModelProvider<TermTaxonomy>>();

    [HttpPost("contact-form")]
    public IActionResult ContactFormSubmit([FromBody] FormUIRequest request)
    {
        var form = new ContactForm(_output);
        form.SetData(request.FormData);

        if (form.IsValid())
        {
            var post = _postProvider.Add(new Post
            {
                PostTitle = string.Join(" ", [form.Find("Info.FirstName")?.GetValue<string>() ?? "", form.Find("Info.LastName")?.GetValue<string>() ?? ""]),
                PostType = "contact-form",
            });

            form.SaveModelData(post.Id, _postMetaProvider);

            // Send Mail =======================================================
            var country = form.Find("Info.Country");
            if (country != null)
            {
                foreach (var item in _output.ThemeOption("Countries.ListOfCountries", false, "contact-form").Split("\n"))
                {
                    if (item.Equals(country.GetValue<string>()))
                    {
                        country.SetValue(_output.Trans(item));
                    }
                }
            }

            var contactType = form.Find("Info.TypeOfInquiry");
            var toEmail = "";
            if (contactType != null)
            {
                foreach (var item in _output.ThemeOptionArray("TypeOfInquiry.Items", "contact-form"))
                {
                    if (_output.MetaValue("Code", item.Value).Equals(contactType.GetValue<string>()))
                    {
                        contactType.SetValue(_output.MetaValue("Name", item.Value, true));
                        toEmail = _output.MetaValue("ContactEmail", item.Value);
                    }
                }
            }

            var pageTemplate = form.Find("Info.PageTemplate");
            if (pageTemplate != null)
            {
                var fieldValue = pageTemplate.GetValue<string>();
                if (fieldValue != "39_my_list")
                {
                    form.Find("ProductList")!.Hidden = true;
                }

                // =====================================================
                Dictionary<string, string> pages = new()
                {
                    ["38_inquiries_and_support"] = _output.Trans("Inquiries & Support"),
                    ["39_my_list"] = _output.Trans("My List"),
                };
                if (pages.TryGetValue(fieldValue!, out var template))
                {
                    pageTemplate.SetValue(template);
                }
            }

            if (!string.IsNullOrEmpty(toEmail))
            {
                var mailer = new EmailProvider(_output);
                var subject = $"[Contact] {contactType!.GetValue<string>()!}";

                form.HideFields([
                    "Info.ActionUrl",
                    "Info.TypeOfInquiry",
                    "Info.Agree",
                ]);
                var message = mailer.GetMailContent(subject, form, "wwwroot/theme/Hulane/EmailTemplate.html");

                mailer.Send(subject, message, [toEmail]);
            }
            // =======================================================

            return Json(new { status = "Success" });
        }

        return Json(new { errors = form.GetErrorMessage() });
    }
}
