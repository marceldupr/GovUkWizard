using Dfe.Wizard.Prototype.Models.Questions;

namespace Dfe.Wizard.Prototype.Application.Questionnaires.Questions
{
    public class DoYouSmokeQuestion : BooleanQuestion
    {
        public DoYouSmokeQuestion() : base("DoYouSmokeQuestion", "Do you smoke?", "Select one",
            "Yes", "No", new Validator
            {
                AllowNull = false,
                NullErrorMessage = "Please select one"
            })
        {
        }
    }
}
