using System.Collections.Generic;
using System.Linq;
using Dfe.Wizard.Prototype.Application;
using Dfe.Wizard.Prototype.Models;
using Dfe.Wizard.Prototype.Models.Questions;
using Dfe.Wizard.Prototype.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Wizard.Prototype.Controllers
{
    public class SessionController : Controller
    {
        private const string QUESTIONNAIRE_SESSION_KEY = "current-questionnaire";
        private const string PROMPT_QUESTIONS = "new-promptquestions-ref";
        private const string FILE_LIST = "files-list";

        protected Questionnaire GetQuestionnaire()
        {
            return HttpContext.Session.Get<Questionnaire>(QUESTIONNAIRE_SESSION_KEY);
        }

        protected FilesViewModel GetFiles()
        {
            return HttpContext.Session.Get<FilesViewModel>(FILE_LIST);
        }

        protected void AddFile(FileUploadResult file)
        {
            var files = GetFiles() ?? new FilesViewModel();

            if (files.Files.All(x => x.Id != file.FileId.ToString()))
            {
                files.Files.Add(new FileViewModel
                {
                    Id = file.FileId.ToString(),
                    FileName = file.FileName,
                    FolderName = file.FolderName
                });
            }

            HttpContext.Session.Set(FILE_LIST, files);
        }

        protected void RemoveFile(string fileId)
        {
            var files = GetFiles() ?? new FilesViewModel();

            var file = files.Files.Single(x => x.Id == fileId);
            files.Files.Remove(file);

            HttpContext.Session.Set(FILE_LIST, files);
        }

        protected List<Question> GetQuestions()
        {
            return HttpContext.Session.Get<List<Question>>(PROMPT_QUESTIONS);
        }

        protected void ClearQuestionnaireAndRelated()
        {
            ClearQuestionnaire();
            ClearQuestions();
        }
        protected void ClearQuestions()
        {
            HttpContext.Session.Remove(PROMPT_QUESTIONS);
        }

        protected void ClearQuestionnaire()
        {
            HttpContext.Session.Remove(QUESTIONNAIRE_SESSION_KEY);
            HttpContext.Session.Remove(FILE_LIST);
        }

        protected void SaveQuestions(List<Question> questions)
        {
            HttpContext.Session.Set(PROMPT_QUESTIONS, questions);
        }

        protected void SaveQuestions(ICollection<Question> questions)
        {
            if (questions != null)
            {
                HttpContext.Session.Set(PROMPT_QUESTIONS, questions.ToList());
            }
        }

        protected void SaveAnswer(UserAnswer userAnswer)
        {
            var questionnaire = GetQuestionnaire();
            questionnaire.Answers ??= new List<UserAnswer>();

            var answer = questionnaire.Answers.SingleOrDefault(x => x.QuestionId == userAnswer.QuestionId);
            if (answer == null)
                questionnaire.Answers.Add(userAnswer);
            else
            {
                questionnaire.Answers.Remove(answer);
                questionnaire.Answers.Add(userAnswer);
            }

            SaveQuestionnaire(questionnaire);
        }

        protected void SaveQuestionnaire(Questionnaire questionnaire)
        {
            HttpContext.Session.Set(QUESTIONNAIRE_SESSION_KEY, questionnaire);
        }
    }
}
