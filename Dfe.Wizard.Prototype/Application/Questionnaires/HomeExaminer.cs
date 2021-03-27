using System.Collections.Generic;
using System.Linq;
using Dfe.Wizard.Prototype.Application.Questionnaires.Questions;
using Dfe.Wizard.Prototype.Models.Common;
using Dfe.Wizard.Prototype.Models.Questions;
using Dfe.Wizard.Prototype.Models.Rules;

namespace Dfe.Wizard.Prototype.Application.Questionnaires
{
    public class HomeExaminer : Examiner
    {
        protected override List<Question> GetQuestions(Questionnaire questionnaire)
        {
            var questions = new List<Question>();

            var doYouOwnYourOwnHome = new BooleanQuestion("DoYouOwnYourOwnHome", "Do you own your own home?", "Select one",
                "Yes", "No", new Validator
                {
                    AllowNull = false,
                    NullErrorMessage = "Please select one"
                });

            questions.Add(doYouOwnYourOwnHome);

            if (questionnaire.Answers.Any(x=>x.QuestionId == "DoYouOwnYourOwnHome") &&
                questionnaire.Answers.Single(x=>x.QuestionId == "DoYouOwnYourOwnHome").Value == "1")
            {
                var whatWasThePriceYouPaid = new NumberQuestion("WhatWhasThePriceYouPaid", "What was the price you paid?", "eg. 245000",
                    new Validator
                    {
                        AllowNull = false,
                        NullErrorMessage = "Please enter a price",
                        InValidErrorMessage = "Please enter a valid price",
                        ValidatorType = ValidatorType.Number
                    });

                questions.Add(whatWasThePriceYouPaid);

                var dateOfPurchase = new DateOfPurchaseQuestion();

                questions.Add(dateOfPurchase);
            }

            return questions;
        }

        public override string QuestionnaireId => nameof(HomeQuestionnaire);
        public override ExaminerType ExaminerType => ExaminerType.Normal;

        protected override QuestionnaireOutcome ApplyRule(Questionnaire questionnaire)
        {
            var doYouOwnHomeAnswer = GetAnswer(questionnaire, "DoYouOwnYourOwnHome");
            if (doYouOwnHomeAnswer.Value == "0")
            {
                return new QuestionnaireOutcome(OutcomeStatus.AutoReject, "This survey is only for home owners");
            }

            var whatWhasThePriceYouPaid = GetAnswer(questionnaire, "WhatWhasThePriceYouPaid");
            if (whatWhasThePriceYouPaid.Value.ToIntOrZero() < 100000)
            {
                return new QuestionnaireOutcome(OutcomeStatus.AutoReject, "This survey is only for home owners who purchased homes over 100000");
            }

            return new QuestionnaireOutcome(OutcomeStatus.AutoAccept, "Thanks for your answers. We appreciate your time to take this survey");
        }

        protected override void ApplyOutcomeToQuestionnaire(Questionnaire questionnaire, QuestionnaireOutcome questionnaireOutcome)
        {
            questionnaire.OutcomeStatus = questionnaireOutcome.OutcomeStatus;
            questionnaire.OutcomeMessage = questionnaireOutcome.OutcomeDescription;
        }
    }
}
