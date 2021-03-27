using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;

namespace Dfe.Wizard.Prototype.TagHelpers
{
    /// <summary>
    /// Overrides validation summary generation of DefaultHtmlGenerator.
    /// Modified from https://github.com/dotnet/aspnetcore/blob/master/src/Mvc/Mvc.ViewFeatures/src/DefaultHtmlGenerator.cs
    /// </summary>
    public class HtmlGenerator : DefaultHtmlGenerator
    {
        public HtmlGenerator(
            IAntiforgery antiforgery,
            IOptions<MvcViewOptions> optionsAccessor,
            IModelMetadataProvider metadataProvider,
            IUrlHelperFactory urlHelperFactory,
            HtmlEncoder htmlEncoder,
            ValidationHtmlAttributeProvider validationAttributeProvider)
            : base(antiforgery, optionsAccessor, metadataProvider, urlHelperFactory, htmlEncoder, validationAttributeProvider)
        {
        }

        public override TagBuilder GenerateValidationSummary(
            ViewContext viewContext, bool excludePropertyErrors, string message, string headerTag, object htmlAttributes)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException(nameof(viewContext));
            }

            var viewData = viewContext.ViewData;
            if (!viewContext.ClientValidationEnabled && viewData.ModelState.IsValid)
            {
                // Client-side validation is not enabled to add to the generated element and element will be empty.
                return null;
            }

            if (excludePropertyErrors &&
                (!viewData.ModelState.TryGetValue(viewData.TemplateInfo.HtmlFieldPrefix, out var entryForModel) ||
                 entryForModel.Errors.Count == 0))
            {
                // Client-side validation (if enabled) will not affect the generated element and element will be empty.
                return null;
            }

            TagBuilder messageTag;
            if (string.IsNullOrEmpty(message))
            {
                messageTag = null;
            }
            else
            {
                if (string.IsNullOrEmpty(headerTag))
                {
                    headerTag = viewContext.ValidationSummaryMessageElement;
                }

                messageTag = new TagBuilder(headerTag);
                messageTag.InnerHtml.SetContent(message);
            }

            // If excludePropertyErrors is true, describe any validation issue with the current model in a single item.
            // Otherwise, list individual property errors.
            var modelStates = ValidationHelpers.GetModelStateList(viewData, excludePropertyErrors);

            var htmlSummary = new TagBuilder("ul");
            htmlSummary.AddCssClass("govuk-list");
            htmlSummary.AddCssClass("govuk-error-summary__list");

            foreach (var modelState in modelStates)
            {
                for (var i = 0; i < modelState.Value.Errors.Count; i++)
                {
                    var modelError = modelState.Value.Errors[i];
                    var errorText = ValidationHelpers.GetModelErrorMessageOrDefault(modelError);

                    if (!string.IsNullOrEmpty(errorText))
                    {
                        var errorLink = new TagBuilder("a");
                        errorLink.Attributes.Add("href", $"#{modelState.Key}");
                        errorLink.InnerHtml.SetContent(errorText);

                        var listItem = new TagBuilder("li");
                        listItem.InnerHtml.AppendHtml(errorLink);
                        htmlSummary.InnerHtml.AppendLine(listItem);
                    }
                }
            }

            var summaryBodyDiv = new TagBuilder("div");
            summaryBodyDiv.AddCssClass("govuk-error-summary__body");

            if (messageTag != null)
            {
                summaryBodyDiv.InnerHtml.AppendLine(messageTag);
            }

            summaryBodyDiv.InnerHtml.AppendHtml(htmlSummary);

            if (viewContext.ClientValidationEnabled && !excludePropertyErrors)
            {
                // Inform the client where to replace the list of property errors after validation.
                summaryBodyDiv.MergeAttribute("data-valmsg-summary", "true");
            }

            var heading = new TagBuilder("h2");
            heading.AddCssClass("govuk-error-summary__title");
            heading.Attributes.Add("id", "error-summary-title");
            heading.InnerHtml.Append("There is a problem");

            var summaryDiv = new TagBuilder("div");
            summaryDiv.AddCssClass("govuk-error-summary");
            summaryDiv.Attributes.Add("aria-labelledby", "error-summary-title");
            summaryDiv.Attributes.Add("role", "alert");
            summaryDiv.Attributes.Add("tabindex", "-1");
            summaryDiv.Attributes.Add("data-module", "govuk-error-summary");
            summaryDiv.InnerHtml.AppendHtml(heading);
            summaryDiv.InnerHtml.AppendHtml(summaryBodyDiv);

            var columnDiv = new TagBuilder("div");
            columnDiv.AddCssClass("govuk-grid-column-full");
            columnDiv.InnerHtml.AppendHtml(summaryDiv);

            var rowDiv = new TagBuilder("div");
            rowDiv.Attributes.Add("id", "error-summary-container");
            rowDiv.AddCssClass("govuk-grid-row");
            rowDiv.InnerHtml.AppendHtml(columnDiv);

            if (viewData.ModelState.IsValid)
            {
                rowDiv.AddCssClass("hidden");
            }

            return rowDiv;
        }
    }
}
