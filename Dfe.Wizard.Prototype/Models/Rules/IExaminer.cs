using Dfe.Wizard.Prototype.Models.Questions;

namespace Dfe.Wizard.Prototype.Models.Rules
{
    public interface IExaminer
    {
        QuestionnaireOutcome Apply(Questionnaire questionnaire);
        string QuestionnaireId { get; }

        ExaminerType ExaminerType { get; }
    }
}
