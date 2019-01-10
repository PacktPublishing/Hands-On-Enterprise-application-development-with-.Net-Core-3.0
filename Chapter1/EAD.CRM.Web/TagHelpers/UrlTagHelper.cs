using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace EAD.CRM.Web.TagHelpers
{
    public class UrlTagHelper : TagHelper
    {
        public string RedirectTo { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            if (!RedirectTo.StartsWith("http://") |
                !RedirectTo.StartsWith("https://"))
            {
                RedirectTo = $"http://{RedirectTo}";
            }
            output.Attributes.SetAttribute("href", RedirectTo);
            output.Attributes.SetAttribute("target", "_blank");
            output.Content.SetContent(RedirectTo);
        }
    }
}
