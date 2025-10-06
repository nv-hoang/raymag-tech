using RefineCMS.Attributes;
using RefineCMS.Common;
using RefineCMS.Common.UI;

namespace RefineCMS.Theme.Raymag.Classes.CustomUI;

[Hook("Theme_Options_FormUI")]
public class OptionTheme(Output _output) : IFilterHook<FormUI>
{
    public FormUI Apply(FormUI form, params object[] args)
    {
        var OptionType = (string)args[0];
        if (OptionType != "theme-options") return form;

        var _ = _output.Trans;
        var Languages = _output.GetLanguages();

        form.Fields = [
            new Group {
                Name = "Info",
                Label = _("General"),
                Childrens = [
                    new LangGroup {
                        Name = "SiteTitle",
                        Label = _("Site Title"),
                        Field = new Text { },
                        Languages = Languages
                    },
                    new LangGroup {
                        Name = "Logo",
                        Label = _("Logo"),
                        Field = new UploadFile {},
                        Languages = Languages
                    },
                    new UploadFile {
                        Name = "Favicon",
                        Label = _("Favicon")
                    },
                ]
            },
            new Group {
                Name = "ContactInfo",
                Label = _("Contact Information"),
                Childrens = [
                    new LangGroup {
                        Name = "Contact",
                        Label = _("Contact"),
                        Field = new Textarea { },
                        Languages = Languages
                    },
                    new Text {
                        Name = "Phone",
                        Label = _("Phone")
                    },
                    new Text {
                        Name = "Fax",
                        Label = _("Fax")
                    },
                    new LangGroup {
                        Name = "Address",
                        Label = _("Address"),
                        Field = new Text {},
                        Languages = Languages
                    },
                    new Text {
                        Name = "Email",
                        Label = _("Email")
                    },
                ]
            },
            new Group {
                Name = "QuickMenu",
                Label = _("Quick Menu"),
                Childrens = [
                    new Repeater {
                        Name = "MenuItems",
                        Label = "Items",
                        Childrens = [
                            new UploadFile {
                                Name = "MenuIcon",
                                Label = _("Icon"),
                                ImageOnly = true
                            },
                            new Text {
                                Name = "MenuLink",
                                Label = _("Link")
                            },
                        ]
                    }
                ]
            },
            new Group {
                Name = "Footer",
                Label = _("Footer"),
                Childrens = [
                    new LangGroup {
                        Name = "Copyright",
                        Label = _("Copyright"),
                        Field = new Text {},
                        Languages = Languages
                    },
                ],
            },
            new Group {
                Name = "Scripts",
                Label = _("Scripts"),
                Childrens = [
                    new Textarea {
                        Name = "Head",
                        Label = _("Head Scripts"),
                        Rows = 10,
                    },
                    new Textarea {
                        Name = "Body",
                        Label = _("Body Scripts"),
                        Rows = 10,
                    },
                ],
            }
        ];

        return form;
    }
}
