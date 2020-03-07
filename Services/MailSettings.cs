using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebLeague.Services
{
    public class MailSettings
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string FromAddress { get; set; }
    }
}
