namespace Dfe.Wizard.Prototype.Models.Questions
{
    public class StringQuestion : Question
    {
        public StringQuestion(string id, string title, string label, Validator validator)
        {
            Id = id;
            Title = title;
            QuestionType = QuestionType.String;
            Validator = validator;
            Answer = new Answer
            {
                Label = label
            };
        }
    }
}
