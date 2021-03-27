using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.Wizard.Prototype.TagHelpers
{
    /// <summary>
    /// Parent div for a set of radio button elements. Applies validation markup as necessary.
    /// </summary>
    [HtmlTargetElement("div", Attributes = MarkupConstants.Classes.AspFor + ",[class^='govuk-radios']")]
    public class RadiosDivTagHelper : TagHelper
    {
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName(MarkupConstants.Classes.AspFor)]
        public ModelExpression For { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            ModelStateEntry entry;

            ViewContext.ViewData.ModelState.TryGetValue(For.Name, out entry);

            if (entry != null && entry.Errors.Count > 0)
            {
                output.PreElement.AppendFormat(
                    MarkupConstants.Elements.SpanErrorMessageFormat, For.Name, entry.Errors[0].ErrorMessage);
            }
        }
    }
}
