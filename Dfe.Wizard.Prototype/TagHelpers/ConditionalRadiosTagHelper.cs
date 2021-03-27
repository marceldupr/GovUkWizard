using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.Wizard.Prototype.TagHelpers
{
    /// <summary>
    /// Parent div for conditional radios. Applies validation markup as necessary.
    /// </summary>
    [HtmlTargetElement("div", Attributes = MarkupConstants.Classes.AspFor + ",[class^='govuk-radios__conditional']")]

    public class ConditionalRadiosTagHelper: TagHelper
    {
        [HtmlAttributeNotBound]
        [ViewContext]

        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName(MarkupConstants.Classes.AspFor)]

        public ModelExpression For { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            ModelStateEntry entry;

            ViewContext.ViewData.ModelState.TryGetValue(For.Name, out entry);

            if (entry != null && entry.Errors.Count > 0)
            {
                var builder = new TagBuilder("div");
                builder.AddCssClass(MarkupConstants.Classes.GroupError);
                output.MergeAttributes(builder);
            }
        }
    }
}
