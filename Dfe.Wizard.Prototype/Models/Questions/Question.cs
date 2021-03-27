using System;
using System.Collections.Generic;
using System.Linq;
using Dfe.Wizard.Prototype.Models.Rules;

namespace Dfe.Wizard.Prototype.Models.Questions
{
    public class Question : IValidatable
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string HelpTextHtml { get; set; }
        public QuestionType QuestionType { get; set; }

        public Validator Validator { get; set; }

        public Answer Answer { get; set; }

        public bool IsValid()
        {
            if (Validator == null)
                throw new ArgumentNullException("Validator not specified");

            return Validator.IsValid();
        }

        public List<string> Validate(string answer)
        {
            if (Validator == null)
                throw new ArgumentNullException("Validator not specified");

            return Validator.Validate(answer);
        }

        protected AnswerPotential GetAnswerPotential(string value)
        {
            if (Answer.AnswerPotentials != null && Answer.AnswerPotentials.Count > 0)
            {
                var answer = Answer.AnswerPotentials.SingleOrDefault(x => x.Value == value);
                if (answer != null) return answer;
            }

            return new AnswerPotential {Description = value, Value = value, Reject = false};
        }
    }
}
