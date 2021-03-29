using Dfe.Wizard.Prototype.Models.Questions;

namespace Dfe.Wizard.Prototype.Application.Questionnaires.Questions
{
    public class DoYouDrinkAlcoholQuestion : BooleanQuestion
    {
        public DoYouDrinkAlcoholQuestion()
            : base(nameof(DoYouDrinkAlcoholQuestion), "Do you drink alcohol?", "Select one", "Yes", "No", new Validator
            {
                AllowNull = false,
                NullErrorMessage = "Select one"
            })
        {
        }
    }
}
