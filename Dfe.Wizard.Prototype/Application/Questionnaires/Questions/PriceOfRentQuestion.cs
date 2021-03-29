using Dfe.Wizard.Prototype.Models.Questions;

namespace Dfe.Wizard.Prototype.Application.Questionnaires.Questions
{
    public class PriceOfRentQuestion : NumberQuestion
    {
        public PriceOfRentQuestion() : base(
            nameof(PriceOfRentQuestion), "What is your current rent per month?", "eg. 1250",
            new Validator
            {
                AllowNull = false,
                NullErrorMessage = "Please enter an amount",
                InValidErrorMessage = "Please enter a valid amount",
                ValidatorType = ValidatorType.Number
            })
        {
        }
    }
}
