using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Dfe.Wizard.Prototype.Models.Questions;

namespace Dfe.Wizard.Prototype.Models.ViewModels
{
    public class HomeViewModel
    {
        public string SelectedQuestionnaire { get; set; }
        public IEnumerable<Questionnaire> Questionnaires { get; set; }
    }
}
