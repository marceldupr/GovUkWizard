using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Dfe.Wizard.Prototype.Models.Questions;

namespace Dfe.Wizard.Prototype.Models.Rules
{
    public class QuestionnaireOutcome
    {
        [AllowNull]
        public List<Question> FurtherQuestions { get; set; }
        [AllowNull]
        public IDictionary<string, ICollection<string>> ValidationErrors { get; set; }
        public bool IsComplete { get; set; }
        public string OutcomeDescription { get; set; }
        public OutcomeStatus OutcomeStatus { get; set; }

        public QuestionnaireOutcome(OutcomeStatus status, string outcomeDescription)
        {
            OutcomeStatus = status;
            IsComplete = true;
            FurtherQuestions = null;
            OutcomeDescription = outcomeDescription;
        }

        public QuestionnaireOutcome(OutcomeStatus status)
        {
            OutcomeStatus = status;
            IsComplete = true;
            FurtherQuestions = null;
        }

        public QuestionnaireOutcome(List<Question> questions, Dictionary<string, ICollection<string>> errors)
        {
            IsComplete = false;
            FurtherQuestions = questions;
            ValidationErrors = errors;
            OutcomeStatus = OutcomeStatus.AwaitingValidationPass;
        }

        public QuestionnaireOutcome(List<Question> furtherQuestion)
        {
            if (furtherQuestion == null)
            {
                FurtherQuestions = null;
                IsComplete = true;
            }
            else
            {
                FurtherQuestions = furtherQuestion;
                OutcomeStatus = OutcomeStatus.AwaitingValidationPass;
                IsComplete = false;
            }

        }
    }
}
