using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Frontend.Models.Entities;
using Frontend.Models.ViewModels;
using Frontend.Models.DTOs;
using Frontend.Services;
using System.Security.Claims;
using System.Text.Json;

namespace Frontend.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly ISimulationService _simulationService;
        private readonly ILogger<StudentController> _logger;
        private readonly Data.ApplicationDbContext _context; 

        public StudentController(
            ISimulationService simulationService, 
            ILogger<StudentController> logger,
            Data.ApplicationDbContext context)
        {
            _simulationService = simulationService;
            _logger = logger;
            _context = context;
        }

        private int GetUserId()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(userIdStr, out int userId);
            return userId;
        }

        public async Task<IActionResult> Dashboard()
        {
            int userId = GetUserId();
            if (userId == 0) return Unauthorized();
            
            var enrollments = await _context.Enrollments
                .Include(e => e.ClassGroup)
                .ThenInclude(c => c!.Teacher)
                .Where(e => e.StudentId == userId)
                .ToListAsync();

            var scenarios = await _simulationService.GetMyScenariosAsync(userId);

            // Load challenges from all classes the student is enrolled in
            var classIds = enrollments.Select(e => e.ClassGroupId).ToList();
            var challenges = await _context.ChallengeAssignments
                .Include(a => a.Challenge)
                .Where(a => classIds.Contains(a.ClassroomId))
                .Select(a => a.Challenge)
                .Where(c => c != null)
                .Cast<Challenge>()
                .ToListAsync();

            var viewModel = new StudentDashboardViewModel
            {
                Classes = enrollments.Select(e => e.ClassGroup).Where(c => c != null).Cast<Classroom>().ToList(),
                Scenarios = scenarios,
                Challenges = challenges
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateScenario(CreateScenarioViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                _logger.LogWarning("Invalid scenario data: {Errors}", errors);
                TempData["ErrorMessage"] = "Invalid scenario data: " + errors;
                return RedirectToAction("Dashboard");
            }

            int userId = GetUserId();
            await _simulationService.CreateScenarioAsync(userId, model.FamilyName, (float)model.InitialBalance);

            TempData["SuccessMessage"] = "Scenario created successfully!";
            return RedirectToAction("Dashboard");
        }

        public async Task<IActionResult> ScenarioDetails(int id)
        {
            int userId = GetUserId();
            var scenario = await _simulationService.GetScenarioDetailsAsync(id, userId);

            if (scenario == null) return NotFound();

            var latestHistory = scenario.Histories.OrderByDescending(h => h.ExecutionDate).FirstOrDefault();
            CalculationResponseDto? lastResult = null;
            if (latestHistory != null)
            {
                try {
                    lastResult = JsonSerializer.Deserialize<CalculationResponseDto>(latestHistory.ResultsJson);
                } catch {}
            }

            var viewModel = new ScenarioDetailsViewModel
            {
                Scenario = scenario,
                EntryTypes = await _context.EntryTypes.ToListAsync(),
                Categories = await _context.Categories.ToListAsync(),
                LatestResult = lastResult,
                LastRunDate = latestHistory?.ExecutionDate,
                AddEntry = new AddEntryViewModel { ScenarioId = id },
                AddObjective = new AddObjectiveViewModel { ScenarioId = id },
                LoanSimulation = new LoanParamsDto()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEntry(AddEntryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                _logger.LogWarning("Invalid entry data for Scenario {ScenarioId}: {Errors}", model.ScenarioId, errors);
                TempData["ErrorMessage"] = "Invalid entry data: " + errors;
                return RedirectToAction("ScenarioDetails", new { id = model.ScenarioId });
            }

            int userId = GetUserId();
            var entryType = _context.EntryTypes.Find(model.TypeId);
            var typeStr = entryType?.Type ?? "EXPENSE";
            
            var success = await _simulationService.AddEntryAsync(
                model.ScenarioId, 
                userId, 
                typeStr, 
                model.CategoryId, 
                model.Amount, 
                model.Month, 
                model.Recurrence ? "True" : "False");

            if (success)
                TempData["SuccessMessage"] = "Entry added successfully!";
            else
                TempData["ErrorMessage"] = "Failed to add entry.";

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

            int userId = GetUserId();
            var success = await _simulationService.AddObjectiveAsync(
                model.ScenarioId, 
                userId, 
                model.Description, 
                (float)model.TargetValue, 
                model.TargetMonths);

            if (success)
                TempData["SuccessMessage"] = "Objective added successfully!";
            else
                TempData["ErrorMessage"] = "Failed to add objective.";

            return RedirectToAction("ScenarioDetails", new { id = model.ScenarioId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateScenario(int id, string familyName, float initialBalance)
        {
            int userId = GetUserId();
            var scenario = await _context.Scenarios.FirstOrDefaultAsync(s => s.Id == id && s.StudentId == userId);
            if (scenario == null) return NotFound();

            scenario.FamilyName = familyName;
            scenario.InitialBalance = initialBalance;

            _context.Scenarios.Update(scenario);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Scenario updated successfully!";
            return RedirectToAction("ScenarioDetails", new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RunSimulation(int scenarioId, LoanParamsDto? loanSimulation)
        {
            int userId = GetUserId();
            
            // If loanSimulation is provided with 0 values, treat it as null
            if (loanSimulation != null && loanSimulation.Principal <= 0)
            {
                loanSimulation = null;
            }

            var result = await _simulationService.RunSimulationAsync(scenarioId, userId, loanSimulation);

            if (result != null)
            {
                TempData["SuccessMessage"] = "Simulation completed successfully!";
                return RedirectToAction("ScenarioDetails", new { id = scenarioId });
            }

            TempData["ErrorMessage"] = "Failed to run simulation. Please check your data and try again.";
            return RedirectToAction("ScenarioDetails", new { id = scenarioId });
        }
    }
}
