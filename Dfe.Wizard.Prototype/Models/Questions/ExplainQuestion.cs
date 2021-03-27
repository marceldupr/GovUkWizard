namespace Dfe.Wizard.Prototype.Models.Questions
{
    public class ExplainQuestion : StringQuestion
    {
        public ExplainQuestion(string id, string title, string label, Validator validator) 
            : base(id, title, label, validator)
        {
            QuestionType = QuestionType.Explain;
            Answer = new Answer
            {
                Label = label
            };
        }
    }
}