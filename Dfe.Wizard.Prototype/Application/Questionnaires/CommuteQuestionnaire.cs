using System.Collections.Generic;
using Dfe.Wizard.Prototype.Models.Questions;

namespace Dfe.Wizard.Prototype.Application.Questionnaires
{
    public class CommuteQuestionnaire : Questionnaire
    {
        public override string Id => nameof(CommuteQuestionnaire);
        public override string Name => "Questions about your commute";
    }
}
