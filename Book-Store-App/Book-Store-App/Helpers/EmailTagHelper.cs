using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store_App.Helpers
{
    public class EmailTagHelper : TagHelper
    {
        public string MailTo { get; set; }
        public string Domain { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";

            string fullMail = $"mailto:{MailTo}@{Domain ?? "gmail.com"}";

            output.Attributes.SetAttribute("href", fullMail);

            string ITag = "<i class=\"fas fa-envelope\"></i>";

            output.Content.SetHtmlContent(ITag);
        }
    }
}
