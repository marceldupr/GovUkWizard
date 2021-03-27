using System.Collections.Generic;

namespace Dfe.Wizard.Prototype.Models.Questions
{
    public class ChoiceQuestion : Question
    {
        public ChoiceQuestion(string id, string title, string label, List<AnswerPotential> potentials, Validator validator)
        {
            Title = title;
            Id = id;
            QuestionType = QuestionType.Choice;
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
