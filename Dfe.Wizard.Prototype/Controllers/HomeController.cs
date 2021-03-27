using System.Diagnostics;
using Dfe.Wizard.Prototype.Application.Services;
using Dfe.Wizard.Prototype.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dfe.Wizard.Prototype.Controllers
{
    public class HomeController : SessionController
    {
        private readonly IQuestionnaireService _questionnaireService;

        public HomeController(IQuestionnaireService questionnaireService)
        {
            _questionnaireService = questionnaireService;
        }

        public IActionResult Index()
        {
            var model = new HomeViewModel();
            model.Questionnaires = _questionnaireService.GetQuestionnaires();
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(HomeViewModel homeViewModel)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", "Questions",
                    new {questionnaireId = homeViewModel.SelectedQuestionnaire});
            }

            return View(homeViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}
