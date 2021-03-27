namespace Dfe.Wizard.Prototype.Models.Questions
{
    public class NumberQuestion : Question
    {
        public NumberQuestion(string id, string title, string label, Validator validator)
        {
            Id = id;
            Title = title;
            QuestionType = QuestionType.Number;
            Validator = validator;
            Answer = new Answer
            {
                Label = label
            };
        }
    }
}
