using System.Collections.Generic;
using Dfe.Wizard.Prototype.Models.Questions;
using Dfe.Wizard.Prototype.Models.Rules;

namespace Dfe.Wizard.Prototype.Models.ViewModels.Questions
{
    public class ResultViewModel
    {
        public List<Question> Questions { get; set; }
        public List<UserAnswer> Answers { get; set; }

        public OutcomeStatus Status { get; set; }
        public string OutcomeMessage { get; set; }
    }
}
