using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpMailSender.Models
{
    public class MailInfo
    {
        public string SenderName { get; set; } = String.Empty;
        public List<string> AddressTo { get; set; } = new List<string>();
        public List<string> AddressCC { get; set; } = new List<string>();
        public List<string> AddressBCC { get; set; } = new List<string>();
        public string MainSubject { get; set; } = String.Empty;
        public string MainBody { get; set; } = String.Empty;
        public bool isHTML { get; set; } = false;
        public List<FileAttach> Attachment { get; set; } = new List<FileAttach>();
    }
}
