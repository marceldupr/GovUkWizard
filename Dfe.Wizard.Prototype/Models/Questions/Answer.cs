using System.Collections.Generic;

namespace Dfe.Wizard.Prototype.Models.Questions
{
    public class Answer
    {
        public bool IsConditional { get; set; }
        public bool HasConditional { get;set; }
        public string ConditionalValue { get;set; }
        public Question ConditionalQuestion { get; set; }
        
        public List<AnswerPotential> AnswerPotentials { get; set; }

        public string Label { get;set; }
    }
}
