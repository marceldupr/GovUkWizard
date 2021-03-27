using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.Wizard.Prototype.TagHelpers
{
    /// <summary>
    /// Adds GOV.UK Frontend validation markup to the text input element.
    /// </summary>
    [HtmlTargetElement("input", Attributes = MarkupConstants.Classes.AspFor + ",[type=text]", TagStructure = TagStructure.WithoutEndTag)]
    public class TextInputTagHelper : TagHelper
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
                var builder = new TagBuilder("input");

                builder.AddCssClass(MarkupConstants.Classes.InputError);
                output.MergeAttributes(builder);

                output.PreElement.AppendFormat(
                    MarkupConstants.Elements.SpanErrorMessageFormat,
                    For.Name,
                    entry.Errors[0].ErrorMessage);
            }
        }
    }
}
