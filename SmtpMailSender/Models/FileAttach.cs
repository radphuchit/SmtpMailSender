using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpMailSender.Models
{
    internal class FileAttach
    {
        public byte[] Source { get; set; }
        public string FileName { get; set; } = String.Empty;
    }
}
