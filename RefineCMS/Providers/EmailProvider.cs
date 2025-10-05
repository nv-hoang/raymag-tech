using MimeKit;
using RefineCMS.Common;
using RefineCMS.Common.UI;

namespace RefineCMS.Providers;

public class EmailProvider(Output _output)
{
    protected string Host = _output.ThemeOption("SMTP.Host", false, "mail-smtp");
    protected string Port = _output.ThemeOption("SMTP.Port", false, "mail-smtp");
    protected string Username = _output.ThemeOption("SMTP.Username", false, "mail-smtp");
    protected string Password = _output.ThemeOption("SMTP.Password", false, "mail-smtp");

    public void Send(string subject, string body, List<string> Emails)
    {
        Task.Run(async () =>
        {
            try
            {
                var email = new MimeMessage();

                email.From.Add(new MailboxAddress("", Username));

                for (int i = 0; i < Emails.Count; i++)
                {
                    if (i == 0)
                    {
                        email.To.Add(new MailboxAddress("", Emails[i]));
                    }
                    else
                    {
                        email.Cc.Add(new MailboxAddress("", Emails[i]));
                    }
                }

                email.Subject = subject;
                email.Body = new TextPart("html") { Text = body };

                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                await smtp.ConnectAsync(Host, int.Parse(Port));
                await smtp.AuthenticateAsync(Username, Password); // App Password
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                Helpers.PrintObject.Out("Send Mail Success...");
            }
            catch (Exception ex)
            {
                Helpers.PrintObject.Out($"Email send failed: {ex.Message}");
            }
        });
    }

    public string GetMailContent(string subject, FormUI form, string template)
    {
        string html = File.ReadAllText(template);
        string fieldHtml = File.ReadAllText(Path.Combine(Path.GetDirectoryName(template)!, "EmailFieldTemplate.html"));

        html = html.Replace("[Subject]", subject);
        var rows = "";
        foreach (var field in form.Find("Info")!.Childrens)
        {
            if (!field.Hidden)
            {
                if (field.Name == "Message")
                {
                    html = html.Replace("[Message]", $"{field.GetValue()}");
                }
                else
                {
                    var row = fieldHtml;
                    row = row.Replace("[FieldLabel]", field.Label);
                    row = row.Replace("[FieldValue]", $"{field.GetValue()}");

                    rows += row;
                }
            }
        }

        html = html.Replace("[Fields]", rows);

        return html;
    }
}
