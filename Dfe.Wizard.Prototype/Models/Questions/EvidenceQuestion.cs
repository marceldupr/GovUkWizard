namespace Dfe.Wizard.Prototype.Models.Questions
{
    public class EvidenceQuestion : Question
    {
        public EvidenceQuestion(string id, string title, string helpTextHtml)
        {
            Id = id;
            Title = title;
            QuestionType = QuestionType.Evidence;
            HelpTextHtml = helpTextHtml;
            Answer = new Answer
            {
                Label = "EvidenceFolderName"
            };
            Validator = new Validator
            {
                AllowNull  = false,
                NullErrorMessage = "Choose a file",
                ValidatorType = ValidatorType.None
            };
        }
    }
}