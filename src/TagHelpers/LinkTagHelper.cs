using System;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CoreWebpackManifest.TagHelpers
{

    // Usage: <link webpack-chunk="app"/>
    [HtmlTargetElement("link", TagStructure = TagStructure.WithoutEndTag)]
    public class LinkTagHelper: TagHelper
    {
        public string WebpackChunk { get; set; }

        private WebpackManifest _manifest;

        public LinkTagHelper(WebpackManifest manifest)
        {
            _manifest = manifest;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.SetAttribute("href", _manifest.GetPath(this.WebpackChunk));
            output.Attributes.SetAttribute("rel", "stylesheet");
        }
    }   
}