using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefineCMS.Attributes;
using RefineCMS.Common;
using RefineCMS.Common.UI;
using RefineCMS.Models;
using RefineCMS.Providers;
using RefineCMS.Requests;

namespace RefineCMS.Controllers.Admin;

[AdminController("users")]
[Auth]
public class UserController(AppFactory _appFactory) : CRUDController(_appFactory)
{
    protected readonly ModelProvider<User> _userProvider = _appFactory.Create<ModelProvider<User>>();
    protected readonly ModelProvider<UserMeta> _userMetaProvider = _appFactory.Create<ModelProvider<UserMeta>>();
    protected readonly ModelProvider<Post> _postProvider = _appFactory.Create<ModelProvider<Post>>();
    protected override string PageTitle { get; set; } = "User";
    protected override string PluralPageTitle { get; set; } = "Users";

    protected override TableUI GetTableUI()
    {
        var _ = _output.Trans;
        return new TableUI
        {
            Api = Url.Action("ApiList"),
            AddNew = new ButtonUI { Label = _($"Add New"), Link = Url.Action("Add") },
            Filters = [
                new TableFilter {
                    Name = "Post_UserStatus",
                    Items = {
                        { "", _("All Status") },
                        { "1", _("Enable") },
                        { "0", _("Disable") }
                    },
                },
            ],
            Columns = [
                new TableColumn {
                    Name = "DisplayName",
                    Title = _("Fullname"),
                    Render = (row) => {
                        var url = Url.Action("Edit", new { id = row["Id"] });
                        return $@"<a href=""{url}"">{row["DisplayName"]}</a>";
                    }
                },
                new TableColumn {
                    Name = "UserLogin",
                    Title = _("Username")
                },
                new TableColumn {
                    Name = "UserEmail",
                    Title = _("Email")
                },
                new TableColumn {
                    Name = "UserRole",
                    Title = _("Role"),
                    Render = (row) => {
                        var RoleId = _output.MetaValue("Info.Role", row);
                        var role = _postProvider.Query().Where(x => x.Id.ToString() == RoleId).FirstOrDefault();
                        return role?.PostTitle ?? "";
                    }
                },
                new TableColumn {
                    Name = "Status",
                    Title = _("Status"),
                    Render = (row) => {
                        return "1".Equals(row["UserStatus"]) ? $@"<a-tag color=""green"">{ _("Enable")}</a-tag>":$@"<a-tag color=""red"">{_("Disable")}</a-tag>";
                    }
                },
                new TableColumn {
                    Name = "CreatedAt",
                    Title = _("Date Created"),
                    Render = (row) => {
                        if(!string.IsNullOrEmpty(_output.MetaValue("Info.CreatedAt", row))) {
                            return row["Info.CreatedAt"];
                        }

                        return string.IsNullOrEmpty(row["CreatedAt"]) ? "" : DateTime.Parse(row["CreatedAt"]).ToString("yyyy-MM-dd (HH:mm)");
                    }
                },
                new TableColumn {
                    Name = "UpdatedAt",
                    Title = _("Last Modified"),
                    Render = (row) => {
                        return string.IsNullOrEmpty(row["UpdatedAt"]) ? "" : DateTime.Parse(row["UpdatedAt"]).ToString("yyyy-MM-dd (HH:mm)");
                    }
                },
            ],
        };
    }

    protected override object GetTableData(TableUIRequest request, TableUI tableUI)
    {
        foreach (var id in request.DeleteIds)
        {
            DeleteUser(id);
        }

        var query = _userProvider.Query();

        if (!string.IsNullOrEmpty(request.Search))
        {
            query = query.Where(x => EF.Functions.Like(x.DisplayName, $"%{request.Search}%") || EF.Functions.Like(x.UserLogin, $"%{request.Search}%"));
        }

        return tableUI.Paginate(request, query, _userMetaProvider, _appFactory);
    }

    protected override bool ValidateQueryId(int id)
    {
        return _userProvider.Exists(id);
    }

    protected override FormUI GetFormUI(int id = 0)
    {
        var _ = _output.Trans;
        var isEdit = id != 0;

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
                            Required = true,
                            Meta = false,
                        },
                        new Email {
                            Name = "UserEmail",
                            Label = _("Email"),
                            Meta = false,
                        },
                        new PostRelationshipSelect {
                            Name = "Role",
                            Label = _("Role"),
                            PostType = "user-role",
                            Factory = _appFactory,
                            Hidden = id == 1
                        }
                    ],
                },
                new Group {
                    Name = "Login",
                    Label = _("Login Setting"),
                    Childrens = [
                        // Add
                        new Text {
                            Hidden = isEdit,
                            Name = "UserLogin",
                            Label = _("Username"),
                            Required = true,
                            Validate = (field, form) => {
                                var user = _userProvider.FirstOrDefault(u => u.UserLogin == field.GetValue<string>());
                                if(user != null) {
                                    field.Message = _("The username already exists.");
                                    return false;
                                }
                                return true;
                            },
                        },
                        new Password {
                            Hidden = isEdit,
                            Name = "UserPass",
                            Label = _("Password"),
                            Required = true,
                            ShowButton = true
                        },

                        // Edit
                        new Text {
                            Hidden = !isEdit,
                            Name = "UserLogin",
                            Label = _("Username"),
                            Disabled = isEdit,
                        },
                        new Password {
                            Hidden = !isEdit,
                            Name = "NewPass",
                            Label = _("Change password"),
                        },
                        new Password {
                            Hidden = !isEdit,
                            Name = "ConfirmPass",
                            Label = _("Re-enter new password"),
                            Validate = (field, form) => {
                                var pass = form.Find("Login.NewPass")?.GetValue<string>();
                                if(!string.IsNullOrEmpty(pass) && !pass.Equals(field.GetValue<string>())) {
                                    field.Message = _("Passwords do not match.");
                                    return false;
                                }
                                return true;
                            },
                        },

                        //
                        new Radio {
                            Name = "UserStatus",
                            Label = _("Status"),
                            Items = {
                                { "1", _("Enable") },
                                { "0", _("Disable") }
                            },
                            Default = "1",
                        },
                    ],
                    Meta = false,
                },
            ],
            BackUrl = Url.Action("Index"),
            Actions = [
                // Add
                new FormAction {
                    Hidden = isEdit,
                    Name = "add",
                    Label = _("Add New"),
                    OnAction = (form) => {
                        var user = AddUser(form);

                        _page.Controller.TempData["MessageSuccess"] = _("Added successfully!");
                        return _page.Controller.RedirectToAction("Edit", new { id = user.Id });
                    }
                },

                // Edit
                new FormAction {
                    Hidden = !isEdit,
                    Name = "save",
                    Label = _("Save Changes"),
                    OnAction = (form) => {
                        UpdateUser(id, form);

                        _page.Controller.TempData["MessageSuccess"] = _("Saved successfully!");
                        return _page.Controller.RedirectToAction("Edit", new { id });
                    }
                },
                new DeleteFormAction {
                    Hidden = !isEdit,
                    Label = _("Delete"),
                    OnAction = (form) => {
                        DeleteUser(id);

                        _page.Controller.TempData["MessageSuccess"] = _("Deleted successfully!");
                        return _page.Controller.RedirectToAction("Index");
                    }
                },
            ]
        };

        if (isEdit)
        {
            var user = _userProvider.FindById(id);
            if (user != null)
            {
                form.LoadModelData(user, _userMetaProvider);
            }
        }

        return form;
    }

    protected User AddUser(FormUI form)
    {
        var user = new User
        {
            DisplayName = form.Find("Info.DisplayName")?.GetValue<string>() ?? "",
            UserEmail = form.Find("Info.UserEmail")?.GetValue<string>() ?? "",

            UserLogin = form.Find("Login.UserLogin")?.GetValue<string>() ?? "",
            UserStatus = form.Find("Login.UserStatus")?.GetValue<int>() ?? 0,
        };

        user.SetPassword(form.Find("Login.UserPass")?.GetValue<string>() ?? "");

        user = _userProvider.Add(user);

        form.SaveModelData(user.Id, _userMetaProvider);

        return user;
    }

    protected User? UpdateUser(int id, FormUI form)
    {
        var user = _userProvider.FindById(id);
        if (user == null) return null;

        user.DisplayName = form.Find("Info.DisplayName")?.GetValue<string>() ?? "";
        user.UserEmail = form.Find("Info.UserEmail")?.GetValue<string>() ?? "";
        user.UserStatus = form.Find("Login.UserStatus")?.GetValue<int>() ?? 0;

        var newPass = form.Find("Login.NewPass")?.GetValue<string>() ?? "";
        if (!string.IsNullOrEmpty(newPass))
        {
            user.SetPassword(newPass);
        }

        form.SaveModelData(id, _userMetaProvider);

        return _userProvider.Update(user);
    }

    protected void DeleteUser(int id)
    {
        _userProvider.Delete(id);
        _userMetaProvider.DeleteMeta(id);
    }
}
