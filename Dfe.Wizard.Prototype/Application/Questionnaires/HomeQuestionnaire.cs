using System.Collections.Generic;
using Dfe.Wizard.Prototype.Models.Questions;

namespace Dfe.Wizard.Prototype.Application.Questionnaires
{
    public class HomeQuestionnaire : Questionnaire
    {
        public override string Id => nameof(HomeQuestionnaire);
        public override string Name => "Questions about your home";
    }
}
