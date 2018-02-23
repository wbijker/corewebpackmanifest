using System;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CoreWebpackManifest.TagHelpers
{

    // Usage: <script webpack-chunk="app"></script>
    [HtmlTargetElement("script")]
    public class ScriptTagHelper: TagHelper
    {
        public string WebpackChunk { get; set; }

        private WebpackManifest _manifest;

        public ScriptTagHelper(WebpackManifest manifest)
        {
            _manifest = manifest;
        }
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.SetAttribute("src", _manifest.GetPath(this.WebpackChunk));
        }
    }   
}