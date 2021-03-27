using System.Collections.Generic;
using Dfe.Wizard.Prototype.Application.Questionnaires.Questions;
using Dfe.Wizard.Prototype.Models.Common;
using Dfe.Wizard.Prototype.Models.Questions;
using Dfe.Wizard.Prototype.Models.Rules;

namespace Dfe.Wizard.Prototype.Application.Questionnaires
{
    public class HomeExaminer : Examiner
    {
        /// <summary>
        /// Question builder. This is iterated on every question. Like a loop. Thus we can alter the order
        /// or add additional questions and remove questions.
        /// </summary>
        /// <param name="questionnaire"></param>
        /// <returns>a list of questions</returns>
        protected override List<Question> GetQuestions(Questionnaire questionnaire)
        {
            var questions = new List<Question>();

            // Ask the user if they own their home
            var ownerOfHomeQuestion = new OwnerOfHomeQuestion();
            questions.Add(ownerOfHomeQuestion);

            // If the user has answered yes to owning a home, then ask these additional questions
            if (GetAnswer(questionnaire, nameof(OwnerOfHomeQuestion))?.Value == "1")
            {
                questions.Add(new PriceOfPurchaseQuestion());
                questions.Add(new DateOfPurchaseQuestion());
            }

            return questions;
        }

        /// <summary>
        /// The QuestionnaireId. Safest way is to use the classname with the 'nameof' function as it is a strong name
        /// </summary>
        public override string QuestionnaireId => nameof(HomeQuestionnaire);
        /// <summary>
        /// This is an example property of how different Examiners can be of certain types.
        /// Then they can be filtered on in the Questionnaire Service by type.
        /// </summary>
        public override ExaminerType ExaminerType => ExaminerType.Normal;

        /// <summary>
        /// This is where the business rules are executed. This only happens after all questions were answered
        /// </summary>
        /// <param name="questionnaire"></param>
        /// <returns>QuestionnaireOutcome</returns>
        protected override QuestionnaireOutcome ApplyRule(Questionnaire questionnaire)
        {
            // If they aren't a home owner then reject the survey
            if (GetAnswer(questionnaire, nameof(OwnerOfHomeQuestion))?.Value == "0")
            {
                return new QuestionnaireOutcome(OutcomeStatus.AutoReject, "This survey is only for home owners");
            }

            // If their purchase price is less than 100K then they are not the target of the survey
            if (GetAnswer(questionnaire, nameof(PriceOfPurchaseQuestion))?.Value.ToIntOrZero() < 100000)
            {
                return new QuestionnaireOutcome(OutcomeStatus.AutoReject, "This survey is only for home owners who purchased homes over 100000");
            }

            // If they answered all the questions then survey is accepted
            return new QuestionnaireOutcome(OutcomeStatus.AutoAccept, "Thanks for your answers. We appreciate your time to take this survey");
        }

        /// <summary>
        /// After all is done, the outcome message and outcome status is applied to the questionnaire
        /// </summary>
        /// <param name="questionnaire"></param>
        /// <param name="questionnaireOutcome"></param>
        protected override void ApplyOutcomeToQuestionnaire(Questionnaire questionnaire, QuestionnaireOutcome questionnaireOutcome)
        {
            // Set the outcome status and message to the questionnaire
            questionnaire.OutcomeStatus = questionnaireOutcome.OutcomeStatus;
            questionnaire.OutcomeMessage = questionnaireOutcome.OutcomeDescription;
        }
    }
}
