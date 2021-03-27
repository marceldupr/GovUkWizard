using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Dfe.Wizard.Prototype.TagHelpers
{
    /// <summary>
    /// Modified from https://github.com/dotnet/aspnetcore/blob/master/src/Mvc/Mvc.ViewFeatures/src/ValidationHelpers.cs
    /// (Internal class that is used by HtmlGenerator.cs)
    /// </summary>
    internal static class ValidationHelpers
    {
        public static string GetModelErrorMessageOrDefault(ModelError modelError)
        {
            Debug.Assert(modelError != null);

            if (!string.IsNullOrEmpty(modelError.ErrorMessage))
            {
                return modelError.ErrorMessage;
            }

            // Default in the ValidationSummary case is no error message.
            return string.Empty;
        }

        public static string GetModelErrorMessageOrDefault(
            ModelError modelError,
            ModelStateEntry containingEntry,
            ModelExplorer modelExplorer)
        {
            Debug.Assert(modelError != null);
            Debug.Assert(containingEntry != null);
            Debug.Assert(modelExplorer != null);

            if (!string.IsNullOrEmpty(modelError.ErrorMessage))
            {
                return modelError.ErrorMessage;
            }

            // Default in the ValidationMessage case is a fallback error message.
            var attemptedValue = containingEntry.AttemptedValue ?? "null";
            return modelExplorer.Metadata.ModelBindingMessageProvider.ValueIsInvalidAccessor(attemptedValue);
        }

        // Returns non-null dictionary of model states, which caller will render in order provided.
        public static IDictionary<string, ModelStateEntry> GetModelStateList(
            ViewDataDictionary viewData,
            bool excludePropertyErrors)
        {
            if (excludePropertyErrors)
            {
                viewData.ModelState.TryGetValue(viewData.TemplateInfo.HtmlFieldPrefix, out var ms);

                if (ms != null)
                {
                    return new Dictionary<string, ModelStateEntry>() { { viewData.TemplateInfo.HtmlFieldPrefix, ms } };
                }
            }
            else if (viewData.ModelState.Count > 0)
            {
                var metadata = viewData.ModelMetadata;
                var modelStateDictionary = viewData.ModelState;
                var entries = new Dictionary<string, ModelStateEntry>();
                Visit(viewData.TemplateInfo.HtmlFieldPrefix, modelStateDictionary.Root, metadata, entries);

                if (entries.Count < modelStateDictionary.Count)
                {
                    // Account for entries in the ModelStateDictionary that do not have corresponding ModelMetadata values.
                    foreach (var entry in modelStateDictionary)
                    {
                        if (!entries.ContainsKey(entry.Key))
                        {
                            entries.Add(entry.Key, entry.Value);
                        }
                    }
                }

                return entries;
            }

            return new Dictionary<string, ModelStateEntry>();
        }

        private static void Visit(
            string parentName,
            ModelStateEntry modelStateEntry,
            ModelMetadata metadata,
            IDictionary<string, ModelStateEntry> orderedModelStateEntries)
        {
            var fieldName = string.IsNullOrWhiteSpace(parentName) ? metadata.Name : $"{parentName}.{metadata.Name}";

            if (metadata.ElementMetadata != null && modelStateEntry.Children != null)
            {
                foreach (var indexEntry in modelStateEntry.Children)
                {
                    Visit(fieldName, indexEntry, metadata.ElementMetadata, orderedModelStateEntries);
                }
            }
            else
            {
                for (var i = 0; i < metadata.Properties.Count; i++)
                {
                    var propertyMetadata = metadata.Properties[i];
                    var propertyModelStateEntry = modelStateEntry.GetModelStateForProperty(propertyMetadata.PropertyName);
                    if (propertyModelStateEntry != null)
                    {
                        Visit(fieldName, propertyModelStateEntry, propertyMetadata, orderedModelStateEntries);
                    }
                }
            }

            if (!modelStateEntry.IsContainerNode)
            {
                orderedModelStateEntries.Add(fieldName, modelStateEntry);
            }
        }
    }
}
