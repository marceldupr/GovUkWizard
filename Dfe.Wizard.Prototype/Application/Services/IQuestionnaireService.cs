using System;
using System.Collections.Generic;
using Dfe.Wizard.Prototype.Models.Questions;
using Dfe.Wizard.Prototype.Models.Rules;

namespace Dfe.Wizard.Prototype.Application.Services
{
    public interface IQuestionnaireService
    {
        IEnumerable<Questionnaire> GetQuestionnaires();
        Questionnaire Start(string questionnaireId);
        QuestionnaireOutcome Iterate(Questionnaire questionnaire);
    }
}
