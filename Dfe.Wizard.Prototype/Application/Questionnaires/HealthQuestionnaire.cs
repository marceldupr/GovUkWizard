using System.Collections.Generic;
using Dfe.Wizard.Prototype.Models.Questions;
using Dfe.Wizard.Prototype.Models.Rules;

namespace Dfe.Wizard.Prototype.Application.Questionnaires
{
    public class HealthQuestionnaire : Questionnaire
    {
        public override string Id => nameof(HealthQuestionnaire);
        public override string Name => "Questions about your health";
    }
}
