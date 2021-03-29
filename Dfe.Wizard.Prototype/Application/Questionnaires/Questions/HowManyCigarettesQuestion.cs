using Dfe.Wizard.Prototype.Models.Questions;

namespace Dfe.Wizard.Prototype.Application.Questionnaires.Questions
{
    public class HowManyCigarettesQuestion : NumberQuestion
    {
        public HowManyCigarettesQuestion()
            : base(nameof(HowManyCigarettesQuestion), "How many cigarettes do you smoke a day?", "eg. 12", new Validator
            {
                AllowNull = false,
                NullErrorMessage = "Please enter an amount",
                ValidatorType = ValidatorType.Number,
                InValidErrorMessage = "Please enter a valid amount"
            })
        {
        }
    }
}
