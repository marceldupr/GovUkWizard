using System.Collections.Generic;
using Dfe.Wizard.Prototype.Models.Questions;

namespace Dfe.Wizard.Prototype.Application.Questionnaires.Questions
{
    public class DateMovedInQuestion : NullableDateQuestion
    {
        public DateMovedInQuestion()
        {
            Setup();
        }

        public void Setup()
        {
            SetupParentYesNo(
                nameof(DateMovedInQuestion),
                "Do you know the date you moved in?",
                "Enter a date", new List<AnswerPotential>
                {
                    new() {Value = "1", Description = "Yes"},
                    new() {Value = "2", Description = "No"}
                }, new Validator
                {
                    InValidErrorMessage = "Please enter a historical date",
                    NullErrorMessage = "Please enter a date",
                    ValidatorType = ValidatorType.DateTimeHistorical,
                    AllowNull = true
                });

            var dateTimeQuestion = new DateTimeQuestion(
                nameof(DateOfPurchaseQuestion),
                "Date tenancy start",
                "eg. 10/10/1990",
                new Validator {ValidatorType = ValidatorType.None});

            SetupDateQuestion(dateTimeQuestion);
        }
    }
}
