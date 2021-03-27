using Dfe.Wizard.Prototype.Models.Questions;

namespace Dfe.Wizard.Prototype.Application.Questionnaires.Questions
{
    public class OwnerOfHomeQuestion : BooleanQuestion
    {
        public OwnerOfHomeQuestion() :
            base(nameof(OwnerOfHomeQuestion), "Do you own your own home?", "Select one",
                "Yes", "No", new Validator
                {
                    AllowNull = false,
                    NullErrorMessage = "Please select one"
                })
        {
        }
    }
}
