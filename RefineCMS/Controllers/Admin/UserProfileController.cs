using Microsoft.AspNetCore.Mvc;
using RefineCMS.Attributes;
using RefineCMS.Common;
using RefineCMS.Common.UI;
using RefineCMS.Extensions;
using RefineCMS.Models;
using RefineCMS.Providers;
using RefineCMS.Requests;

namespace RefineCMS.Controllers.Admin;

[AdminController("profile")]
[Auth]
public class UserProfileController(AppFactory _appFactory) : BaseController(_appFactory)
{
    protected readonly ModelProvider<User> _userProvider = _appFactory.Create<ModelProvider<User>>();
    protected readonly ModelProvider<UserMeta> _userMetaProvider = _appFactory.Create<ModelProvider<UserMeta>>();

    [HttpGet]
    public IActionResult Index()
    {
        var user = _page.User;
        if (user == null) return NotFound();

        var formUI = GetFormUI(user);
        ViewData["FormUI"] = formUI;
        ViewData["Title"] = _output.Trans($"Edit Profile");
        return _page.Template("Admin/FormView");
    }

    [HttpPost]
    public virtual IActionResult IndexFormSubmit(FormUIRequest request)
    {
        var user = _page.User;
        if (user == null) return NotFound();

        var formUI = GetFormUI(user);
        formUI.SetData(request.FormData);
        if (formUI.IsValid())
        {
            return (IActionResult)formUI.DoAction(request.SubmitAction)!;
        }

        TempData["MessageError"] = _output.Trans("Invalid request, please check again.");
        ViewData["FormUI"] = formUI;
        ViewData["Title"] = _output.Trans($"Edit Profile");
        return _page.Template("Admin/FormView");
    }

    protected FormUI GetFormUI(User user)
    {
        var _ = _output.Trans;
        var form = new FormUI
        {
            Fields = [
                new Group {
                    Name = "Info",
                    Label = _("User Information"),
                    Childrens = [
                        new Text {
                            Name = "DisplayName",
                            Label = _("Fullname"),
                            Required = true
                        },
                        new Email {
                            Name = "UserEmail",
                            Label = _("Email")
                        },
                    ],
                    Meta = false,
                },
                new Group {
                    Name = "Login",
                    Label = _("Login Setting"),
                    Childrens = [
                        new Text {
                            Name = "UserLogin",
                            Label = _("Username"),
                            Disabled = true
                        },
                        new Password {
                            Name = "NewPass",
                            Label = _("Change password")
                        },
                        new Password {
                            Name = "ConfirmPass",
                            Label = _("Re-enter new password"),
                            Validate = (field, form) => {
                                var pass = form.Find("Login.NewPass")?.GetValue<string>();
                                if(!string.IsNullOrEmpty(pass) && !pass.Equals(field.GetValue<string>())) {
                                    field.Message = _("Passwords do not match.");
                                    return false;
                                }
                                return true;
                            }
                        },
                    ],
                    Meta = false,
                },
            ],
            Actions = [
                new FormAction {
                    Name = "save",
                    Label = _("Save Changes"),
                    OnAction = (form) => {
                        UpdateProfile(form);

                        _page.Controller.TempData["MessageSuccess"] = _("Saved successfully!");
                        return _page.Controller.RedirectToAction("Index");
                    }
                },
            ]
        };

        form.LoadModelData(user, _userMetaProvider);

        return form;
    }

    protected User? UpdateProfile(FormUI form)
    {
        var user = _page.User;
        if (user == null) return null;

        user.DisplayName = form.Find("Info.DisplayName")?.GetValue<string>() ?? "";
        user.UserEmail = form.Find("Info.UserEmail")?.GetValue<string>() ?? "";

        var newPass = form.Find("Login.NewPass")?.GetValue<string>() ?? "";
        if (!string.IsNullOrEmpty(newPass))
        {
            user.SetPassword(newPass);
        }

        var userUpdated = _userProvider.Update(user);

        form.SaveModelData(userUpdated.Id, _userMetaProvider);

        HttpContext.Session.SetObject("user", userUpdated);
        return userUpdated;
    }
}
