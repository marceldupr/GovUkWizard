using System.Collections.Generic;
using Dfe.Wizard.Prototype.Models.Questions;
using Dfe.Wizard.Prototype.Models.Rules;

namespace Dfe.Wizard.Prototype.Application.Questionnaires
{
    public class CommuteExaminer : Examiner
    {
        protected override List<Question> GetQuestions(Questionnaire questionnaire)
        {
            throw new System.NotImplementedException();
        }

        public override string QuestionnaireId => nameof(CommuteQuestionnaire);
        public override ExaminerType ExaminerType => ExaminerType.Normal;


        protected override QuestionnaireOutcome ApplyRule(Questionnaire questionnaire)
        {
            throw new System.NotImplementedException();
        }

        protected override void ApplyOutcomeToQuestionnaire(Questionnaire questionnaire, QuestionnaireOutcome questionnaireOutcome)
        {
            throw new System.NotImplementedException();
        }
    }
}
