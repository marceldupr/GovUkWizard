using System.Collections.Generic;
using Dfe.Wizard.Prototype.Models.Questions;

namespace Dfe.Wizard.Prototype.Application.Questionnaires.Questions
{
    public class DateOfPurchaseQuestion : NullableDateQuestion
    {
        public DateOfPurchaseQuestion()
        {
            Setup();
        }

        public void Setup()
        {
            SetupParentYesNo(
                nameof(DateOfPurchaseQuestion),
                "Do you know the date of purchase?",
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
                "Date of purchase",
                "eg. 10/10/1990",
                new Validator {ValidatorType = ValidatorType.None});

            SetupDateQuestion(dateTimeQuestion);
        }
    }
}
