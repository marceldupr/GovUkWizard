using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Dfe.Wizard.Prototype.Application.Services;
using Dfe.Wizard.Prototype.Models;
using Dfe.Wizard.Prototype.Models.Questions;
using Dfe.Wizard.Prototype.Models.Rules;
using Dfe.Wizard.Prototype.Models.ViewModels.Questions;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Wizard.Prototype.Controllers
{
    public class QuestionsController : SessionController
    {
        private readonly IEvidenceService _evidenceService;
        private readonly IQuestionnaireService _questionnaireService;
        private long ThreeMegabytes => 3000000;

        public QuestionsController(IEvidenceService evidenceService, IQuestionnaireService questionnaireService)
        {
            _evidenceService = evidenceService;
            _questionnaireService = questionnaireService;
        }

        public IActionResult Index(string questionnaireId)
        {
            var questionnaire = _questionnaireService.Start(questionnaireId);
            SaveQuestionnaire(questionnaire);
            return RedirectToAction("Prompt");
        }

        public IActionResult Result()
        {
            var questionnaire = GetQuestionnaire();
            var resultViewModel = new ResultViewModel
            {
                Answers = questionnaire.Answers,
                Questions = GetQuestions(),
                Status = questionnaire.OutcomeStatus,
                OutcomeMessage = questionnaire.OutcomeMessage
            };
            return View("Result", resultViewModel);
        }

        public IActionResult Prompt(int currentIndex=0)
        {
            var questionnaire = GetQuestionnaire();

            var questionnaireOutcome = _questionnaireService.Iterate(questionnaire);

            SaveQuestionnaire(questionnaire);

            if (questionnaireOutcome.IsComplete || questionnaireOutcome.FurtherQuestions == null)
            {
                SaveQuestionnaire(questionnaire);

                return RedirectToAction("Result");
            }

            var questions = questionnaireOutcome.FurtherQuestions.ToList();
            SaveQuestions(questions);

            var promptViewModel = new QuestionViewModel(questions, currentIndex);

            return View("Prompt", promptViewModel);
        }

        [HttpPost]
        public IActionResult Prompt(PromptAnswerViewModel promptAnswerViewModel, bool Continue)
        {
            var questions = GetQuestions();
            var thisQuestion = FindQuestion(questions, promptAnswerViewModel.QuestionId);

            var promptAnswer = promptAnswerViewModel.GetAnswerAsString(Request.Form);
            SaveAnswer(new UserAnswer{ QuestionId = promptAnswerViewModel.QuestionId, Value = promptAnswer });

            var questionnaire = GetQuestionnaire();

            if (thisQuestion.QuestionType == QuestionType.Evidence)
            {
                if (Request.Form.Files.Count > 0)
                {
                    var result = UploadEvidence(questionnaire);

                    if (result != null)
                    {
                        var errorUploadViewModel = CreateValidationErrorUploadModel(promptAnswerViewModel, result,
                            questions);

                        return View("Prompt", errorUploadViewModel);
                    }
                }
                else if (!Continue && Request.Form.ContainsKey("remove"))
                {
                    var fileId = Request.Form["remove"];
                    var file = GetFiles().Files.Single(x => x.Id == fileId);
                    _evidenceService.DeleteEvidenceFile(Guid.Parse(file.Id));

                    RemoveFile(fileId);
                    var uploadEvidenceViewModel = CreateUploadMoreViewModel(promptAnswerViewModel, questions,
                        questionnaire);

                    ViewBag.HasAdded = !Request.Form.ContainsKey("skipValidation");
                    return View("Prompt", uploadEvidenceViewModel);
                }
                else if(!Continue && !Request.Form.ContainsKey("skipValidation"))
                {
                    var errorUploadViewModel = CreateErrorUploadViewModel(promptAnswerViewModel, thisQuestion,
                        questions);

                    return View("Prompt", errorUploadViewModel);
                }

                if (!Continue)
                {
                    var uploadEvidenceViewModel = CreateUploadMoreViewModel(promptAnswerViewModel, questions,
                        questionnaire);

                    ViewBag.HasAdded = !Request.Form.ContainsKey("skipValidation");
                    return View("Prompt", uploadEvidenceViewModel);
                }

                SaveAnswer(new UserAnswer{ QuestionId = promptAnswerViewModel.QuestionId, Value = questionnaire.EvidenceFolderName });
            }

            questionnaire = GetQuestionnaire();

            var outcome = _questionnaireService.Iterate(questionnaire);

            if (outcome.FurtherQuestions != null)
            {
                SaveQuestions(outcome.FurtherQuestions);
                questions = GetQuestions();
            }

            if (NotNullableDateIsSelected(thisQuestion))
            {
                var errorsPromptViewModel = CreateNullableDateErrorPrompt(promptAnswerViewModel,
                    thisQuestion, questions);

                return View("Prompt", errorsPromptViewModel);
            }

            if (NullableDateQuestionHasErrors(thisQuestion))
            {
                var errorsPromptViewModel = CreateNullableInvalidDateErrorPrompt(promptAnswerViewModel,
                    thisQuestion, questions);

                return View("Prompt", errorsPromptViewModel);
            }

            if (GenericQuestionHasErrors(outcome))
            {
                var errorsPromptViewModel = CreateGenericQuestionErrorPrompt(promptAnswerViewModel, questions,
                    outcome, thisQuestion);

                return View("Prompt", errorsPromptViewModel);
            }

            var nextQuestionViewModel = GoToTheNextQuestion(promptAnswerViewModel, questions, questionnaire);

            if (nextQuestionViewModel.HasMoreQuestions)
            {
                return View("Prompt", nextQuestionViewModel);
            }

            SaveQuestionnaire(questionnaire);

            return RedirectToAction("Result");
        }

        private QuestionViewModel CreateUploadMoreViewModel(PromptAnswerViewModel promptAnswerViewModel, List<Question> questions,
            Questionnaire questionnaire)
        {
            var uploadEvidenceViewModel = new QuestionViewModel(questions, promptAnswerViewModel.CurrentIndex);

            ViewBag.Upload = GetFiles();
            ViewBag.HasAdded = !Request.Form.ContainsKey("skipValidation");
            return uploadEvidenceViewModel;
        }

        private QuestionViewModel CreateErrorUploadViewModel(PromptAnswerViewModel promptAnswerViewModel, Question thisQuestion,
            List<Question> questions)
        {
            ViewData.ModelState.AddModelError(promptAnswerViewModel.QuestionId, thisQuestion.Validator.NullErrorMessage);
            ViewData["errorMessage"] = thisQuestion.Validator.NullErrorMessage;

            var errorUploadViewModel = new QuestionViewModel(questions, promptAnswerViewModel.CurrentIndex);

            ViewBag.Upload = GetFiles();
            return errorUploadViewModel;
        }

        internal QuestionViewModel CreateValidationErrorUploadModel(PromptAnswerViewModel promptAnswerViewModel, FileValidationError error, List<Question> questions)
        {
            ViewData.ModelState.AddModelError(promptAnswerViewModel.QuestionId, error.Title);
            ViewData["errorMessage"] = error.Detail;

            var errorUploadViewModel = new QuestionViewModel(questions, promptAnswerViewModel.CurrentIndex);

            ViewBag.Upload = GetFiles();
            return errorUploadViewModel;
        }

        private FileValidationError UploadEvidence(Questionnaire questionnaire)
        {
            var file = Request.Form.Files.First();
            var whitelist = ".jpg,.doc,.docx,.pdf,.html".Split(',');

            if (!whitelist.Any(x => file.FileName.EndsWith(x, true, CultureInfo.InvariantCulture)))
            {
                return new FileValidationError("The file is not an acceptable file format","The file is not an acceptable file format - acceptable file format is .JPG, .DOC, .DOCX, PDF, .HTML");
            }

            if (file.Length > ThreeMegabytes)
            {
                return new FileValidationError("The file size can not be more than 3MB", "The file size can not be more than 3MB");
            }

            var files = GetFiles();

            if(files?.Files != null && files.Files.Count >= 12)
            {
                return new FileValidationError("Only 12 files allowed", "There is only a maximum of 12 files allowable. Please remove file(s) before uploading more evidence.");
            }

            if (string.IsNullOrEmpty(questionnaire.EvidenceFolderName))
            {
                var fileUploadResult = _evidenceService.UploadEvidence(file);

                questionnaire.EvidenceFolderName = fileUploadResult.FolderName;
                AddFile(fileUploadResult);
                SaveQuestionnaire(questionnaire);
            }
            else
            {
                var fileUploadResult = _evidenceService.UploadEvidence(questionnaire.EvidenceFolderName, file);

                AddFile(fileUploadResult);
            }

            return null;
        }

        private static QuestionViewModel GoToTheNextQuestion(PromptAnswerViewModel promptAnswerViewModel, List<Question> questions,
            Questionnaire questionnaire)
        {
            var nextQuestionViewModel = new QuestionViewModel(questions, promptAnswerViewModel.CurrentIndex + 1);
            return nextQuestionViewModel;
        }

        private bool NotNullableDateIsSelected(Question thisQuestion)
        {
            return thisQuestion.QuestionType == QuestionType.NullableDate && string.IsNullOrEmpty(Request.Form[thisQuestion.Id]);
        }

        private bool NullableDateQuestionHasErrors(Question thisQuestion)
        {
            return thisQuestion.QuestionType == QuestionType.NullableDate &&
                   Request.Form[thisQuestion.Id] == "1" &&
                   string.IsNullOrEmpty(Request.Form["date-day"]) &&
                   string.IsNullOrEmpty(Request.Form["date-month"]) &&
                   string.IsNullOrEmpty(Request.Form["date-year"]);
        }

        private static bool GenericQuestionHasErrors(QuestionnaireOutcome outcome)
        {
            return outcome.ValidationErrors != null && outcome.ValidationErrors.Count > 0;
        }

        private QuestionViewModel CreateGenericQuestionErrorPrompt(PromptAnswerViewModel promptAnswerViewModel, List<Question> questions,
            QuestionnaireOutcome outcome, Question thisQuestion)
        {
            var errorsPromptViewModel =
                new QuestionViewModel(questions, promptAnswerViewModel.CurrentIndex, outcome.ValidationErrors)
                {
                    ShowConditional = thisQuestion.Answer.IsConditional
                };

            string actualMessage = string.Empty;
            foreach (var errorMessage in outcome.ValidationErrors)
            {
                foreach (var promptError in errorMessage.Value)
                {
                    ViewData.ModelState.AddModelError(errorMessage.Key, promptError);
                    actualMessage = promptError;
                }
            }

            if (!string.IsNullOrEmpty(actualMessage))
            {
                ViewData["errorMessage"] = actualMessage;
            }

            return errorsPromptViewModel;
        }

        private QuestionViewModel CreateNullableInvalidDateErrorPrompt(PromptAnswerViewModel promptAnswerViewModel,
            Question thisQuestion, List<Question> questions)
        {
            ViewData.ModelState.AddModelError(promptAnswerViewModel.QuestionId, thisQuestion.Validator.NullErrorMessage);
            ViewData["errorMessage"] = thisQuestion.Validator.NullErrorMessage;

            List<string> errorCollection = new List<string> {thisQuestion.Validator.NullErrorMessage};
            var validationErrors = new Dictionary<string, ICollection<string>>
            {
                {promptAnswerViewModel.QuestionId, errorCollection}
            };

            var errorsPromptViewModel = new QuestionViewModel(questions, promptAnswerViewModel.CurrentIndex, validationErrors)
            {
                ShowConditional = thisQuestion.Answer.IsConditional
            };
            return errorsPromptViewModel;
        }

        private QuestionViewModel CreateNullableDateErrorPrompt(PromptAnswerViewModel promptAnswerViewModel,
            Question thisQuestion, List<Question> questions)
        {
            ViewData.ModelState.AddModelError(promptAnswerViewModel.QuestionId, thisQuestion.Answer.Label);
            ViewData["errorMessage1"] = thisQuestion.Answer.Label;
            ViewData["errorType"] = "NoneSelected";

            List<string> errorCollection = new List<string> {thisQuestion.Answer.Label};
            var validationErrors = new Dictionary<string, ICollection<string>>
            {
                {promptAnswerViewModel.QuestionId, errorCollection}
            };

            var errorsPromptViewModel = new QuestionViewModel(questions, promptAnswerViewModel.CurrentIndex, validationErrors)
            {
                ShowConditional = thisQuestion.Answer.IsConditional
            };
            return errorsPromptViewModel;
        }

        private Question FindQuestion(IList<Question> questions, string questionId)
        {
            var thisQuestion = questions.SingleOrDefault(x => x.Id == questionId);
            if (thisQuestion == null)
            {
                return questions.Select(x => x.Answer.ConditionalQuestion).Single(x => x != null && x.Id == questionId);
            }

            return thisQuestion;
        }
    }
}
