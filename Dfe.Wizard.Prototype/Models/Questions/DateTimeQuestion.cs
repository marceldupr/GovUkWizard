namespace Dfe.Wizard.Prototype.Models.Questions
{
    public class DateTimeQuestion : Question
    {
        public DateTimeQuestion(string id, string title, string label, Validator validator)
        {
            Id = id;
            Title = title;
            QuestionType = QuestionType.DateTime;
            Validator = validator;
            Answer = new Answer
            {
                Label = label
            };
        }
    }
}
