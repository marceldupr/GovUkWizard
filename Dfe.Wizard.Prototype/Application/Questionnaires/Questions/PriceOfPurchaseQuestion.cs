using Dfe.Wizard.Prototype.Models.Questions;

namespace Dfe.Wizard.Prototype.Application.Questionnaires.Questions
{
    public class PriceOfPurchaseQuestion : NumberQuestion
    {
        public PriceOfPurchaseQuestion() : base(
            nameof(PriceOfPurchaseQuestion), "What was the price you paid?", "eg. 245000",
            new Validator
            {
                AllowNull = false,
                NullErrorMessage = "Please enter a price",
                InValidErrorMessage = "Please enter a valid price",
                ValidatorType = ValidatorType.Number
            })
        {
        }
    }
}
