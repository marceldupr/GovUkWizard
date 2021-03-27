using System;
using System.Linq;
using Dfe.Wizard.Prototype.Models.Questions;
using Microsoft.AspNetCore.Http;

namespace Dfe.Wizard.Prototype.Models.ViewModels.Questions
{
    public class PromptAnswerViewModel
    {
        public string FieldType { get; set; }
        public string QuestionId { get; set; }
        public int CurrentIndex { get; set; }

        public string GetAnswerAsString(IFormCollection fields)
        {
            string answerAsString = string.Empty;

            switch (GetPromptAnswerType())
            {
                case (QuestionType.DateTime):
                    answerAsString = $"{fields["date-day"]}/{fields["date-month"]}/{fields["date-year"]}";
                    break;
                case (QuestionType.NullableDate):
                    answerAsString = $"{fields["date-day"]}/{fields["date-month"]}/{fields["date-year"]}";
                    break;
                case (QuestionType.Number):
                    answerAsString = $"{fields["number"]}";
                    break;
                case (QuestionType.Select):
                    answerAsString = fields["select"];
                    break;
                case (QuestionType.String):
                    answerAsString = fields["string"];
                    break;
                case (QuestionType.Boolean):
                    answerAsString = fields["boolean"];
                    break;
                case (QuestionType.Explain):
                    answerAsString = fields["explain"];
                    break;
            }

            if (answerAsString == "//")
                answerAsString = string.Empty;

            return answerAsString;
        }

        private QuestionType GetPromptAnswerType()
        {
            if (!string.IsNullOrEmpty(FieldType))
            {
                if (Enum.GetNames(typeof(QuestionType)).Any(
                    e => e.Trim().ToUpperInvariant() == FieldType.Trim().ToUpperInvariant()))
                {
                    return (QuestionType) Enum.Parse(typeof(QuestionType), FieldType, true);
                }
            }

            throw new Exception("Could not find Answer Type");
        }
    }
}
