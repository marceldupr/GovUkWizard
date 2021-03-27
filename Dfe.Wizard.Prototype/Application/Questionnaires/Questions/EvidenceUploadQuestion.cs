using Dfe.Wizard.Prototype.Models.Questions;

namespace Dfe.Wizard.Prototype.Application.Questionnaires.Questions
{
    public class EvidenceUploadQuestion : EvidenceQuestion
    {
        public EvidenceUploadQuestion(string title, string helpTextHtml) :
            base(nameof(EvidenceUploadQuestion), title, helpTextHtml)
        {
        }
    }
}
