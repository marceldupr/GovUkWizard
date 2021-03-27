using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.Wizard.Prototype.TagHelpers
{
    /// <summary>
    /// Parent div for date inputs. Applies validation markup as necessary.
    /// </summary>
    [HtmlTargetElement("div", Attributes = MarkupConstants.Classes.AspFor + ",[class^='govuk-date-input']")]
    public class DateDivTagHelper : TagHelper
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
                output.PreElement.AppendFormat(
                    MarkupConstants.Elements.SpanErrorMessageFormat, For.Name, entry.Errors[0].ErrorMessage);

                var childContent = output.Content.IsModified
                    ? output.Content.GetContent() : (await output.GetChildContentAsync()).GetContent();

                const string dateInputClass = "govuk-date-input__input";

                output.Content.SetHtmlContent(Regex.Replace(
                     childContent,
                     dateInputClass,
                     $"{dateInputClass} {MarkupConstants.Classes.InputError}"));
            }
        }
    }
}
