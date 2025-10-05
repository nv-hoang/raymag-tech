using RefineCMS.Attributes;
using RefineCMS.Common;
using RefineCMS.Common.UI;

namespace RefineCMS.Theme.Raymag.Classes.CustomUI;

[Hook("Theme_Options_FormUI")]
public class OptionMail(Output _output) : IFilterHook<FormUI>
{
    public FormUI Apply(FormUI form, params object[] args)
    {
        var OptionType = (string)args[0];
        if (OptionType != "mail-smtp") return form;

        var _ = _output.Trans;
        var Languages = _output.GetLanguages();

        form.Fields = [
            new Group {
                Name = "SMTP",
                Label = _("SMTP Setting"),
                Childrens = [
                    //new Text {
                    //    Name = "Subject",
                    //    Label = _("Subject"),
                    //},
                    new Text {
                        Name = "Host",
                        Label = _("SMTP Host"),
                    },
                    //new Radio {
                    //    Name = "Encryption",
                    //    Label = _("Encryption"),
                    //    Items = {
                    //        { "ssl", _("SSL") },
                    //        { "tls", _("TLS") },
                    //    },
                    //    Default = "ssl"
                    //},
                    new Text {
                        Name = "Port",
                        Label = _("Port"),
                        Default = "465",
                    },
                    new Text {
                        Name = "Username",
                        Label = _("SMTP Username"),
                    },
                    new Password {
                        Name = "Password",
                        Label = _("SMTP Password"),
                    },
                ]
            },
        ];

        return form;
    }
}

