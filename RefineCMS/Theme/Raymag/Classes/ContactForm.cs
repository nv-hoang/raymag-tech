using RefineCMS.Common;
using RefineCMS.Common.UI;

namespace RefineCMS.Theme.Raymag.Classes;

public class ContactForm(Output _output) : FormUI
{
    public override List<FormField> Fields { get; set; } = [
        new Group {
            Name = "Info",
            Childrens = [
                new Text {
                    Name = "PageTemplate",
                    Label = _output.Trans("Page"),
                    Required = true,
                },
                new Text {
                    Name = "ActionUrl",
                    Label = _output.Trans("Url"),
                    Required = true,
                },
                new Text {
                    Name = "FullName",
                    Label = _output.Trans("Full Name"),
                    Required = true,
                },
                new Text {
                    Name = "Phone",
                    Label = _output.Trans("Phone Number"),
                },
                new Text {
                    Name = "Company",
                    Label = _output.Trans("Company"),
                    Required = true,
                },
                new Email {
                    Name = "Email",
                    Label = _output.Trans("E-mail"),
                    Required = true,
                },
                new Textarea {
                    Name = "Message",
                    Label = _output.Trans("Message"),
                    Required = true,
                },
            ]
        },
    ];
}
