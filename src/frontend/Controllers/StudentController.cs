using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Frontend.Models.Entities;
using Frontend.Models.ViewModels;
using Frontend.Models.DTOs;
using Frontend.Services;
using System.Security.Claims;
using System.Text.Json;
using System.Linq;

namespace Frontend.Controllers
{
    [Authorize(Roles = "Student")]
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

        public async Task<IActionResult> Dashboard(int? scenarioId = null)
        {
            int userId = GetUserId();
            if (userId == 0) return Unauthorized();
            
            var enrollments = await _context.Enrollments
                .Include(e => e.ClassGroup)
                .ThenInclude(c => c!.Teacher)
                .Where(e => e.StudentId == userId)
                .ToListAsync();

            var scenarios = await _simulationService.GetMyScenariosAsync(userId);

            // Load histories for the scenarios to get the latest calculation results
            foreach (var scenario in scenarios)
            {
                 scenario.Histories = await _simulationService.GetSimulationHistoryAsync(scenario.Id, userId);
            }

            Scenario? selectedScenario = null;
            if (scenarioId.HasValue)
            {
                selectedScenario = scenarios.FirstOrDefault(s => s.Id == scenarioId.Value);
            }
            if (selectedScenario == null)
            {
                selectedScenario = scenarios.FirstOrDefault();
            }

            var classIds = enrollments.Select(e => e.ClassGroupId).ToList();
            var challenges = await _context.ChallengeAssignments
                .Include(a => a.Challenge)
                .Where(a => classIds.Contains(a.ClassroomId))
                .Select(a => a.Challenge)
                .Where(c => c != null)
                .Cast<Challenge>()
                .ToListAsync();

            CalculationResponseDto? latestResult = null;
            if (selectedScenario != null)
            {
                var latestHistory = selectedScenario.Histories.OrderByDescending(h => h.ExecutionDate).FirstOrDefault();
                if (latestHistory != null && !string.IsNullOrEmpty(latestHistory.ResultsJson))
                {
                    try {
                        latestResult = JsonSerializer.Deserialize<CalculationResponseDto>(latestHistory.ResultsJson);
                    } catch {}
                }
            }

            var viewModel = new StudentDashboardViewModel
            {
                Classes = enrollments.Select(e => e.ClassGroup).Where(c => c != null).Cast<Classroom>().ToList(),
                Scenarios = scenarios,
                Challenges = challenges,
                SelectedScenario = selectedScenario,
                LatestResult = latestResult
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Family()
        {
            int userId = GetUserId();
            var scenarios = await _simulationService.GetMyScenariosAsync(userId);
            return View(scenarios);
        }

        public async Task<IActionResult> Transactions(int? scenarioId = null)
        {
            int userId = GetUserId();
            var scenarios = await _simulationService.GetMyScenariosAsync(userId);
            
            Scenario? selectedScenario = null;
            if (scenarioId.HasValue)
            {
                selectedScenario = await _simulationService.GetScenarioDetailsAsync(scenarioId.Value, userId);
            }
            if (selectedScenario == null && scenarios.Any())
            {
                selectedScenario = await _simulationService.GetScenarioDetailsAsync(scenarios.First().Id, userId);
            }

            var entryTypes = await _context.EntryTypes.ToListAsync();
            var categories = await _context.Categories.ToListAsync();

            ViewBag.Scenarios = scenarios;
            ViewBag.SelectedScenario = selectedScenario;
            
            if (selectedScenario != null)
            {
                ViewBag.AddEntryModel = new AddEntryViewModel 
                { 
                    ScenarioId = selectedScenario.Id,
                    EntryTypes = entryTypes,
                    Categories = categories
                };
            }

            return View(scenarios);
        }

        public async Task<IActionResult> Challenges()
        {
            int userId = GetUserId();
            var enrollments = await _context.Enrollments
                .Where(e => e.StudentId == userId)
                .Select(e => e.ClassGroupId)
                .ToListAsync();

            var challenges = await _context.ChallengeAssignments
                .Include(a => a.Challenge)
                .Where(a => enrollments.Contains(a.ClassroomId))
                .Select(a => a.Challenge)
                .Where(c => c != null)
                .Cast<Challenge>()
                .ToListAsync();

            ViewBag.MyScenarios = await _simulationService.GetMyScenariosAsync(userId);

            return View(challenges);
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

            int userId = GetUserId();
            await _simulationService.CreateScenarioAsync(userId, model.FamilyName, (float)model.InitialBalance);

            TempData["SuccessMessage"] = "Scenario created successfully!";
            return RedirectToAction("Family");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteScenario(int id)
        {
            int userId = GetUserId();
            var success = await _simulationService.DeleteScenarioAsync(id, userId);

            if (success)
            {
                TempData["SuccessMessage"] = "Cenário apagado com sucesso!";
            }
            else
            {
                TempData["ErrorMessage"] = "Não foi possível apagar o cenário.";
            }

            return RedirectToAction("Family");
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

            var entryTypes = await _context.EntryTypes.ToListAsync();
            var categories = await _context.Categories.ToListAsync();

            var viewModel = new ScenarioDetailsViewModel
            {
                Scenario = scenario,
                EntryTypes = entryTypes,
                Categories = categories,
                LatestResult = lastResult,
                LastRunDate = latestHistory?.ExecutionDate,
                AddEntry = new AddEntryViewModel 
                { 
                    ScenarioId = id,
                    EntryTypes = entryTypes,
                    Categories = categories
                },
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
                TempData["ErrorMessage"] = "Invalid entry data.";
                return Redirect(Request.Headers["Referer"].ToString() ?? "/Student/Dashboard");
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
                TempData["SuccessMessage"] = "Lançamento adicionado com sucesso!";
            else
                TempData["ErrorMessage"] = "Falha ao adicionar o lançamento.";

            return Redirect(Request.Headers["Referer"].ToString() ?? "/Student/Dashboard");
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
        public async Task<IActionResult> RunSimulation(int scenarioId, LoanParamsDto? loanSimulation, SavingsParamsDto? savingsSimulation, CashFlowParamsDto? cashFlowSimulation)
        {
            int userId = GetUserId();
            
            if (loanSimulation != null && loanSimulation.Principal <= 0)
            {
                loanSimulation = null;
            }

            if (savingsSimulation != null && savingsSimulation.MonthlyContribution <= 0)
            {
                savingsSimulation = null;
            }

            if (cashFlowSimulation != null && cashFlowSimulation.MonthlyIncome <= 0)
            {
                cashFlowSimulation = null;
            }

            var result = await _simulationService.RunSimulationAsync(scenarioId, userId, loanSimulation, savingsSimulation, cashFlowSimulation);

            if (result != null)
            {
                TempData["SuccessMessage"] = "Simulation completed successfully!";
                return RedirectToAction("ScenarioDetails", new { id = scenarioId });
            }

            TempData["ErrorMessage"] = "Failed to run simulation. Please check your data and try again.";
            return RedirectToAction("ScenarioDetails", new { id = scenarioId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitChallenge(int challengeId, int scenarioId)
        {
            int userId = GetUserId();
            var scenario = await _context.Scenarios.FirstOrDefaultAsync(s => s.Id == scenarioId && s.StudentId == userId);
            
            if (scenario == null)
            {
                TempData["ErrorMessage"] = "Cenário não encontrado.";
                return RedirectToAction("Challenges");
            }

            scenario.ChallengeId = challengeId;
            _context.Scenarios.Update(scenario);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Desafio entregue com sucesso! Aguarde a avaliação do professor.";
            return RedirectToAction("Challenges");
        }
    }
}
