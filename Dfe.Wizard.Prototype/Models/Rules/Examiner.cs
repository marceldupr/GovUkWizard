using System.Collections.Generic;
using System.Linq;
using Dfe.Wizard.Prototype.Models.Questions;

namespace Dfe.Wizard.Prototype.Models.Rules
{
    public abstract class Examiner : IExaminer
    {
        protected abstract List<Question> GetQuestions(Questionnaire questionnaire);

        public abstract string QuestionnaireId { get; }

        public abstract ExaminerType ExaminerType { get; }

        public virtual QuestionnaireOutcome Apply(Questionnaire questionnaire)
        {
            var questions = GetQuestions(questionnaire);

            var errorMessages = new Dictionary<string, ICollection<string>>();

            foreach (var question in questions)
            {
                var answer = questionnaire.Answers.SingleOrDefault(x => x.QuestionId == question.Id);
                if (answer != null)
                {
                    var errors = question.Validate(answer.Value);

                    if (errors.Count > 0)
                    {
                        errorMessages.Add(question.Id, errors);
                    }

                    if (question.Answer.HasConditional)
                    {
                        answer = questionnaire.Answers.SingleOrDefault(x => x.QuestionId == question.Answer.ConditionalQuestion.Id);
                        if (answer != null)
                        {
                            errors = question.Answer.ConditionalQuestion.Validate(answer.Value);

                            if (errors.Count > 0)
                            {
                                errorMessages.Add(question.Answer.ConditionalQuestion.Id, errors);
                            }
                        }
                    }
                }
            }

            if (errorMessages.Count > 0)
            {
                return new QuestionnaireOutcome(questions, errorMessages);
            }

            if (questionnaire.Answers == null || questionnaire.Answers
                .Where(x => !x.QuestionId.Contains("."))
                .Select(x => x.QuestionId)
                .Distinct()
                .Count() < questions.Count)
            {
                return new QuestionnaireOutcome(questions);
            }

            QuestionnaireOutcome questionnaireOutcome = ApplyRule(questionnaire);

            ApplyOutcomeToQuestionnaire(questionnaire, questionnaireOutcome);

            return questionnaireOutcome;
        }

        protected abstract QuestionnaireOutcome ApplyRule(Questionnaire questionnaire);

        protected abstract void ApplyOutcomeToQuestionnaire(Questionnaire questionnaire, QuestionnaireOutcome questionnaireOutcome);

        private bool HasAnswer(Questionnaire questionnaire, string questionId)
        {
            return questionnaire.Answers.Any(x => x.QuestionId == questionId);
        }

        protected UserAnswer GetAnswer(Questionnaire questionnaire, string questionId)
        {
            if (HasAnswer(questionnaire, questionId))
            {
                return questionnaire.Answers.Single(x => x.QuestionId == questionId);
            }

            return null;
        }
    }
}
