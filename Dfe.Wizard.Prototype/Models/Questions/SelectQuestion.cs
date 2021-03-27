using System.Collections.Generic;

namespace Dfe.Wizard.Prototype.Models.Questions
{
    public class SelectQuestion : Question
    {
        public SelectQuestion(string id, string title, string label, List<AnswerPotential> potentials, Validator validator)
        {
            Title = title;
            Id = id;
            QuestionType = QuestionType.Select;
            Validator = validator;
            Answer = new Answer
            {
                Label = label,
                AnswerPotentials = potentials,
                IsConditional = false
            };
        }
    }
}
