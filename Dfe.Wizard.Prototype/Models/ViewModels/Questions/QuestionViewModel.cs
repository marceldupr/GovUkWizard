using System.Collections.Generic;
using System.Linq;
using Dfe.Wizard.Prototype.Models.Questions;

namespace Dfe.Wizard.Prototype.Models.ViewModels.Questions
{
    public class QuestionViewModel
    {
        public string GetTitle()
        {
            return CurrentQuestion.Title;
        }

        public ICollection<Question> Questions { get;set; }

        public int CurrentQuestionIndex { get; set; }

        public bool HasMoreQuestions => Questions.Count > CurrentQuestionIndex;


        public Question CurrentQuestion
        {
            get
            {
                var currentQuestion = Questions.ToList()[CurrentQuestionIndex];
                if (ShowConditional)
                    return currentQuestion.Answer.ConditionalQuestion;

                return currentQuestion;
            }
        }

        public IDictionary<string, ICollection<string>> ValidationErrors { get; set; }

        public bool ShowConditional { get; set; }

        public QuestionViewModel(ICollection<Question> questions, int currentIndex=0, IDictionary<string, ICollection<string>> validationErrors=null)
        {
            Questions = questions;
            CurrentQuestionIndex = currentIndex;
            ValidationErrors = validationErrors;
        }
    }
}
