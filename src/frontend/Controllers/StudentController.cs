using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Frontend.Models;
using Frontend.Services;

namespace Frontend.Controllers
{
    //[Authorize(Roles = "STUDENT")]
    public class StudentController : Controller
    {
        private readonly IEducationService _educationService;
        private readonly ISimulationService _simulationService;

        public StudentController(IEducationService educationService, ISimulationService simulationService)
        {
            _educationService = educationService;
            _simulationService = simulationService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var classes = await _educationService.GetMyClassesAsync();
            var scenarios = await _simulationService.GetMyScenariosAsync();

            ViewBag.Classes = classes;
            ViewBag.Scenarios = scenarios;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> JoinClass(JoinClassViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid join code format.";
                return RedirectToAction("Dashboard");
            }

            var success = await _educationService.JoinClassAsync(model.JoinCode);
            if (success)
            {
                TempData["SuccessMessage"] = "Successfully joined the class!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to join class. Check the code or if you are already enrolled.";
            }

            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateScenario(CreateScenarioViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid scenario data.";
                return RedirectToAction("Dashboard");
            }

            var success = await _simulationService.CreateScenarioAsync(model.FamilyName, model.InitialBalance);
            if (success)
            {
                TempData["SuccessMessage"] = "Scenario created successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create scenario.";
            }

            return RedirectToAction("Dashboard");
        }

        public async Task<IActionResult> ScenarioDetails(int id)
        {
            var scenario = await _simulationService.GetScenarioDetailsAsync(id);
            if (scenario == null) return NotFound();

            return View(scenario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEntry(AddEntryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid entry data.";
                return RedirectToAction("ScenarioDetails", new { id = model.ScenarioId });
            }

            var success = await _simulationService.AddEntryAsync(model.ScenarioId, model.Type, model.Category, model.Amount, model.Month, model.Recurrence);
            if (success)
            {
                TempData["SuccessMessage"] = "Entry added successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to add entry.";
            }

            return RedirectToAction("ScenarioDetails", new { id = model.ScenarioId });
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddObjective(AddObjectiveViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid objective data.";
                return RedirectToAction("ScenarioDetails", new { id = model.ScenarioId });
            }

            var success = await _simulationService.AddObjectiveAsync(model.ScenarioId, model.Description, model.TargetValue, model.TermMonths);
            if (success)
            {
                TempData["SuccessMessage"] = "Objective added successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to add objective.";
            }

            return RedirectToAction("ScenarioDetails", new { id = model.ScenarioId });
        }
    }
}
