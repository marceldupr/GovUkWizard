using System.Collections.Generic;
using Dfe.Wizard.Prototype.Models.Questions;

namespace Dfe.Wizard.Prototype.Application.Questionnaires.Questions
{
    public class UnderlyingConditionQuestion : SelectQuestion
    {
        public UnderlyingConditionQuestion()
            : base(nameof(UnderlyingConditionQuestion),
                "Do you have any of these underlying conditions?",
                "Select one", new List<AnswerPotential>
                {
                    new AnswerPotential{Description = "Diabetes Type 1", Value = "1", Reject = true},
                    new AnswerPotential{Description = "Diabetes Type 2", Value = "2"},
                    new AnswerPotential{Description = "Arterialsclerosis", Value = "3"},
                    new AnswerPotential{Description = "Arterial fibrilation", Value = "4"},
                    new AnswerPotential{Description = "Lupus", Value = "5"},
                    new AnswerPotential{Description = "Multiple Sclerosis", Value = "6"},
                    new AnswerPotential{Description = "ALS", Value = "7"},
                    new AnswerPotential{Description = "None", Value = "8"},
                    new AnswerPotential{Description = "Don't want to say", Value = "9", Reject = true}
                },
                new Validator
                {
                    AllowNull = false,
                    NullErrorMessage = "Please select one"
                })
        {
        }
    }
}
