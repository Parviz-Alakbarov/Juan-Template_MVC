using System.Net.Mail;
using System.Threading.Tasks;

namespace JuanShoesStore.Util
{
    public static class SendEmailUtil
    {
        public async static Task SendEmailUtilAsync(string email, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(email);
            mail.From = new MailAddress(Constants.EmailAddress);
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            string Body = body;
            mail.Body = Body;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(Constants.EmailAddress, Constants.Password);
            smtp.EnableSsl = true;
            try
            {
                await smtp.SendMailAsync(mail);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
