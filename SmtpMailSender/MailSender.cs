using SmtpMailSender.Models;
using System.Net.Mail;
using System.Text;

namespace SmtpMailSender
{
    public class MailSender
    {
        private string defaultSignature = "";

        private string MailServer = "";
        public string Signature
        {
            get
            {
                return defaultSignature;
            }
            set
            {
                defaultSignature = value;
            }
        }

        public MailSender(string MailServer)
        {
            this.MailServer = MailServer;

            var sb = new StringBuilder();
            sb.AppendLine("------------------------------------------------------------------");
            sb.AppendLine("Generate on : " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm") + "  ");
            sb.AppendLine("------------------------------------------------------------------");

            defaultSignature = sb.ToString();
        }

        public bool SendMail(MailInfo mailInfo, int retry = 0)
        {
            MailMessage MyEmail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(this.MailServer);
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;

            MyEmail.From = new MailAddress(mailInfo.SenderName);
            MyEmail.Subject = mailInfo.MainSubject;

            if (mailInfo.AddressTo != null)
            {
                foreach (var temp in mailInfo.AddressTo)
                {
                    if (temp.IndexOf("@") != -1)
                        MyEmail.To.Add(new MailAddress(temp, temp.Substring(0, temp.IndexOf("@"))));
                    else
                        MyEmail.To.Add(new MailAddress(temp));
                }
            }

            if (mailInfo.AddressCC != null)
            {
                foreach (var temp in mailInfo.AddressCC)
                {
                    if (temp.IndexOf("@") != -1)
                        MyEmail.CC.Add(new MailAddress(temp, temp.Substring(0, temp.IndexOf("@"))));
                    else
                        MyEmail.CC.Add(new MailAddress(temp));
                }
            }

            if (mailInfo.AddressBCC != null)
            {
                foreach (var temp in mailInfo.AddressBCC)
                {
                    if (temp.IndexOf("@") != -1)
                        MyEmail.Bcc.Add(new MailAddress(temp, temp.Substring(0, temp.IndexOf("@"))));
                    else
                        MyEmail.Bcc.Add(new MailAddress(temp));
                }
            }

            if (mailInfo.Attachment != null)
            {
                foreach (FileAttach attach in mailInfo.Attachment)
                {
                    Stream stream = new MemoryStream(attach.Source);
                    MyEmail.Attachments.Add(new Attachment(stream, attach.FileName));
                }
            }

            MyEmail.BodyEncoding = ASCIIEncoding.UTF8;
            MyEmail.IsBodyHtml = mailInfo.isHTML;
            MyEmail.BodyTransferEncoding = System.Net.Mime.TransferEncoding.EightBit;

            MyEmail.Body = mailInfo.MainBody;
            if (mailInfo.isHTML)
            {
                MyEmail.Body += this.Signature.Replace("\r\n", "<br>");
            }
            else
            {
                MyEmail.Body += this.Signature;
            }

            try
            {
                SmtpServer.Send(MyEmail);
            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < retry; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        Console.WriteLine("Delivery failed - retrying in 5 seconds.");
                        System.Threading.Thread.Sleep(5000);
                        SmtpServer.Send(MyEmail);
                    }
                    else
                    {
                        Console.WriteLine("Failed to deliver message to {0}",
                            ex.InnerExceptions[i].FailedRecipient);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in RetryIfBusy(): {0}",
                        ex.ToString());
            }
            return true;

        }


    }
}