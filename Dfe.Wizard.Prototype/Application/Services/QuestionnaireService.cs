using System.Collections.Generic;
using System.Linq;
using Dfe.Wizard.Prototype.Models.Questions;
using Dfe.Wizard.Prototype.Models.Rules;

namespace Dfe.Wizard.Prototype.Application.Services
{
    public class QuestionnaireService : IQuestionnaireService
    {
        private readonly IEnumerable<Questionnaire> _questionnaires;
        private readonly IEnumerable<IExaminer> _examiners;

        public QuestionnaireService(IEnumerable<Questionnaire> questionnaires, IEnumerable<IExaminer> examiners)
        {
            _questionnaires = questionnaires;
            _examiners = examiners;
        }

        public Questionnaire Start(string questionnaireId)
        {
            return GetQuestionnaires().Single(x => x.Id == questionnaireId);
        }

        public IEnumerable<Questionnaire> GetQuestionnaires()
        {
            return _questionnaires;
        }

        public QuestionnaireOutcome Iterate(Questionnaire questionnaire)
        {
            // Here you can get all examiners and run them in a chain OR you can use the ExaminerType to find a specific type of Examiner.
            // For the sake of this example we just take the first one that was found.
            var examiners = _examiners.First(x => x.QuestionnaireId == questionnaire.Id);

            return examiners.Apply(questionnaire);
        }
    }
}
