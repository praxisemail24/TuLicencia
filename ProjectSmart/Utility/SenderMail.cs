using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using SmartLicencia.Models;
using SmartLicencia.Repository;
using SmartLicense.Utils;
using System.Text.RegularExpressions;

namespace SmartLicencia.Utility
{
    public class SenderMail
    {
        private readonly IConfiguration _config;
        private readonly PlantillaMensajeRepository _repository;

        public SenderMail(PlantillaMensajeRepository repository, IConfiguration config)
        {
            _repository = repository;
            _config = config;
        }

        public string Send(string templateName, MailPartialBody content)
        {
            var pl = _repository.ObtenerPorNombre(templateName);

            if (pl == null)
                throw new Exception("Error al intentar enviar correo electrónico.");

            return Send(pl, content);
        }

        public string Send(PlantillaMensaje pl, MailPartialBody content)
        {
            content.Variables.Add("from_address", _config.GetSection("Email:UserName").Value);
            content.Variables.Add("from_name", _config.GetSection("Email:Name").Value);

            string subject = string.IsNullOrWhiteSpace(pl.Asunto) ? string.Empty : pl.Asunto;

            List<SenderMailAddress> cc = content.Cc;

            if(!string.IsNullOrWhiteSpace(pl.Cc))
            {
                var toEmails = pl.Cc.Split(",");
                for (var i = 0; i < toEmails.Length; i++)
                {
                    var email = toEmails[i].Split("|");

                    if (email.Length == 2)
                        cc.Add(new SenderMailAddress(email[0], email[1]));

                    if (email.Length == 1)
                        cc.Add(new SenderMailAddress(email[0]));
                }
            }

            return Send(new MailBody
            {
                Subject = subject,
                Body = pl.Contenido,
                Cc = cc,
                To = content.To,
                Files = content.Files,
                Variables = content.Variables,
            });
        }

        public string Send(MailBody content)
        {
            content.Variables.Add("now", DateTime.Now);
            content.Variables.Add("year", DateTime.Now.Year);

            foreach (var item in content.Variables)
            {
                content.Body = content.Body.Replace("{{" + item.Key.Trim().ToUpper() + "}}", item.Value.ToHtml());
                content.Subject = content.Subject.Replace("{{" + item.Key.Trim().ToUpper() + "}}", item.Value.ToHtml());
            }

            var email = new MimeMessage();
            email.From.Add(InternetAddress.Parse(_config.GetSection("Email:UserName").Value));

            foreach (var item in content.To)
                email.To.Add(new MailboxAddress(item.Name, item.Address));

            foreach (var item in content.Cc)
                email.Cc.Add(new MailboxAddress(item.Name, item.Address));

            email.Subject = content.Subject;
            var body = content.Body.StartsWith("<!DOCTYPE html>") ? content.Body : $@"<html>
            <meta content=""text/html; charset=UTF-8"" http-equiv=""Content-Type"" />
            <head>
            <style>
                * {{
                    font-family: 'Inter', sans-serif;
                }}
            </style>
            </head>
            <body>{content.Body}</body></html>";

            Multipart multipart = new Multipart("mixed");
            multipart.Add(new TextPart(TextFormat.Html) { Text = body, });

            foreach (var file in content.Files)
                multipart.Add(Attach(file.Key, file.Value));

            email.Body = multipart;

            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            smtp.Connect(
                _config.GetSection("Email:Host").Value,
                Convert.ToInt32(_config.GetSection("Email:Port").Value),
                SecureSocketOptions.StartTls
            );

            smtp.Authenticate(_config.GetSection("Email:UserName").Value, _config.GetSection("Email:Password").Value);
            var sended = smtp.Send(email);
            smtp.Disconnect(true);

            return sended;
        }

        public MimePart Attach(string fileName, string mimeType)
        {
            var contentType = mimeType.Split("/");

            return new MimePart(contentType[0], contentType[1])
            {
                Content = new MimeContent(File.OpenRead(fileName)),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = Path.GetFileName(fileName)
            };
        }

        public bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }
    }

    public class MailPartialBody
    {
        public List<SenderMailAddress> To { get; set; }
        public List<SenderMailAddress> Cc { get; set; }
        public Dictionary<string, string> Files { get; set; }
        public Dictionary<string, object> Variables { get; set; }

        public MailPartialBody()
        {
            To = new List<SenderMailAddress>();
            Cc = new List<SenderMailAddress>();
            Files = new Dictionary<string, string>();
            Variables = new Dictionary<string, object>();
        }
    }

    public class MailBody : MailPartialBody
    {
        public string Subject { get; set; }
        public string Body { get; set; }

        public MailBody() 
        {
            Subject = string.Empty;
            Body = string.Empty;
        }
    }

    public class SenderMailAddress
    {
        public string Name { get; set; }
        public string Address { get; set; }

        public SenderMailAddress()
        {
            Name = string.Empty;
            Address = string.Empty;
        }

        public SenderMailAddress(string address, string name = "")
        {
            Name = string.IsNullOrWhiteSpace(name) ? address : name;
            Address = address;
        }
    }
}
