using System.Collections.Generic;
using System.Linq;
using Dfe.Wizard.Prototype.Application.Questionnaires.Questions;
using Dfe.Wizard.Prototype.Models.Common;
using Dfe.Wizard.Prototype.Models.Questions;
using Dfe.Wizard.Prototype.Models.Rules;

namespace Dfe.Wizard.Prototype.Application.Questionnaires
{
    public class HealthExaminer : Examiner
    {
        protected override List<Question> GetQuestions(Questionnaire questionnaire)
        {
            var questions = new List<Question>();

            var doYouSmokeQuestion = new DoYouSmokeQuestion();
            questions.Add(doYouSmokeQuestion);

            if (GetAnswer(questionnaire, nameof(DoYouSmokeQuestion))?.Value == "1")
            {
                questions.Add(new HowManyCigarettesQuestion());
            }

            var doYouDrinkQuestion = new DoYouDrinkAlcoholQuestion();
            questions.Add(doYouDrinkQuestion);

            if (GetAnswer(questionnaire, nameof(DoYouDrinkAlcoholQuestion))?.Value == "1")
            {
                questions.Add(new HowManyDrinksPerDayQuestion());
            }

            var underlyingHealthConditionQuestion = new UnderlyingConditionQuestion();
            questions.Add(underlyingHealthConditionQuestion);

            var details = new ExplainQuestion(nameof(ExplainQuestion), "What medications are you one at the moment?",
                "Please list", new Validator
                {
                    AllowNull = true,
                    ValidatorType = ValidatorType.MaxCharacters,
                    ValidatorCompareValue = "500",
                    InValidErrorMessage = "Please stay under 500 characters"
                });
            questions.Add(details);

            return questions;
        }

        public override string QuestionnaireId => nameof(HealthQuestionnaire);
        public override ExaminerType ExaminerType => ExaminerType.Normal;

        protected override QuestionnaireOutcome ApplyRule(Questionnaire questionnaire)
        {
            if (GetAnswer(questionnaire, nameof(HowManyCigarettesQuestion))?.Value.ToIntOrZero() > 20)
            {
                return new QuestionnaireOutcome(OutcomeStatus.AutoReject, "Unfortunately, you cannot join the marathon");
            }

            if (GetAnswer(questionnaire, nameof(HowManyDrinksPerDayQuestion))?.Value.ToIntOrZero() > 10)
            {
                return new QuestionnaireOutcome(OutcomeStatus.AutoReject, "Unfortunately, you cannot join the marathon");
            }

            if (new[] {"3", "4", "6", "7"}.ToList().Any(x => x == GetAnswer(questionnaire, nameof(UnderlyingConditionQuestion))?.Value))
            {
                return new QuestionnaireOutcome(OutcomeStatus.AutoReject, "Unfortunately, you cannot join the marathon");
            }

            if (GetAnswer(questionnaire, nameof(ExplainQuestion))?.Value != string.Empty)
            {
                return new QuestionnaireOutcome(OutcomeStatus.AwatingReview, "We will review your application to join the marathon");
            }

            return new QuestionnaireOutcome(OutcomeStatus.AutoAccept, "Your application was successful, you can join the marathon");
        }

        protected override void ApplyOutcomeToQuestionnaire(Questionnaire questionnaire, QuestionnaireOutcome questionnaireOutcome)
        {
            questionnaire.OutcomeStatus = questionnaireOutcome.OutcomeStatus;
            questionnaire.OutcomeMessage = questionnaireOutcome.OutcomeDescription;
        }
    }
}
