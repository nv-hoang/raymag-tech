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
                PostTitle = form.Find("Info.FullName")?.GetValue<string>() ?? "",
                PostType = "contact-form",
            });

            form.SaveModelData(post.Id, _postMetaProvider);

            // Send Mail =======================================================
            var toEmail = _output.ThemeOption("ContactInfo.Email");

            if (!string.IsNullOrEmpty(toEmail))
            {
                var mailer = new EmailProvider(_output);
                var subject = "Contact";

                form.HideFields([
                    "Info.ActionUrl",
                ]);
                var message = mailer.GetMailContent(subject, form, "wwwroot/theme/Raymag/EmailTemplate.html");

                mailer.Send(subject, message, [toEmail]);
            }
            // =======================================================

            return Json(new { status = "Success" });
        }

        return Json(new { errors = form.GetErrorMessage() });
    }
}
