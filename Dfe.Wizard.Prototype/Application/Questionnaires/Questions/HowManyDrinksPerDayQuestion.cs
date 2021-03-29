using Dfe.Wizard.Prototype.Models.Questions;

namespace Dfe.Wizard.Prototype.Application.Questionnaires.Questions
{
    public class HowManyDrinksPerDayQuestion : NumberQuestion
    {
        public HowManyDrinksPerDayQuestion()
            : base(nameof(HowManyDrinksPerDayQuestion), "How many drinks per day?", "One drink equals 1 beer, 1 glass of wine (125ml) or 1 shot of spirits", new Validator
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
