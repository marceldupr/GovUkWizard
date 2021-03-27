using System.Collections.Generic;
using Dfe.Wizard.Prototype.Models.Rules;

namespace Dfe.Wizard.Prototype.Models.Questions
{
    public class Questionnaire
    {
        public Questionnaire()
        {
            Answers = new List<UserAnswer>();
        }

        public virtual string Id { get; set; }
        public virtual string Name { get; set; }
        public List<UserAnswer> Answers { get; set; }
        public string EvidenceFolderName { get; set; }
        public OutcomeStatus OutcomeStatus { get; set; }
        public string OutcomeMessage { get; set; }
    }
}
