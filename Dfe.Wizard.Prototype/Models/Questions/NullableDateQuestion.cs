using System.Collections.Generic;

namespace Dfe.Wizard.Prototype.Models.Questions
{
    public abstract class NullableDateQuestion : Question 
    {
        protected virtual void SetupParentYesNo(string id, string title, string label, List<AnswerPotential> answers, Validator validator)
        {
            Title = title;
            Id = id;
            QuestionType = QuestionType.NullableDate;
            Answer = Answer = new Answer
            {
                Label = label,
                AnswerPotentials = answers
            };
            Validator = validator;
        }

        protected virtual void SetupDateQuestion(Question question)
        {
            Answer.ConditionalQuestion = question;
        }
    }
}
