using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store_App.Models
{
    public class MailRequestModel
    {
        public List<string> ToEmails { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public List<KeyValuePair<string, string>> PlaceHolders { get; set; }
    }
}
