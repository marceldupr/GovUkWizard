using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.Wizard.Prototype.TagHelpers
{
    [HtmlTargetElement("script", Attributes = "webpack-src")]
    public class WebpackTagHelper : TagHelper
    {
        private const string ScriptBundleRootPath = "assets/scripts/build";
        private IWebHostEnvironment _webHostEnvironment;

        public string WebpackSrc { get; set; }

        public WebpackTagHelper(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.SetAttribute("src", GetScriptBundleUrl(WebpackSrc));
        }

        private string GetScriptBundleUrl(string prefix)
        {
            var rootPath = ScriptBundleRootPath;

            var mappedPath = Path.Combine(_webHostEnvironment.WebRootPath, ScriptBundleRootPath);

            var file = Path.GetFileName(
                Directory.EnumerateFiles(mappedPath, $"{prefix}*.js").OrderBy(p => p).FirstOrDefault()
                ?? throw new Exception($"File not found (prefix={prefix},extension=js)"));
            var fileUrl = $"/{rootPath}/{file}";

            return fileUrl;
        }
    }
}
