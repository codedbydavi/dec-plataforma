using Microsoft.EntityFrameworkCore;
using Frontend.Data;
using Frontend.Models.Entities;
using Frontend.Models.DTOs;
using System.Text.Json;

namespace Frontend.Services
{
    public interface ISimulationService
    {
        Task<List<Scenario>> GetMyScenariosAsync(int studentId);
        Task<Scenario?> GetScenarioDetailsAsync(int scenarioId, int studentId);
        Task<Scenario> CreateScenarioAsync(int studentId, string familyName, float initialBalance);
        Task<bool> DeleteScenarioAsync(int scenarioId, int studentId);
        Task<bool> AddEntryAsync(int scenarioId, int studentId, string type, int categoryId, float amount, string month, string recurrence);
        Task<bool> UpdateEntryAsync(int entryId, int studentId, int categoryId, float amount, string month, string recurrence);
        Task<bool> DeleteEntryAsync(int entryId, int studentId);
        Task<bool> AddObjectiveAsync(int scenarioId, int studentId, string description, float targetValue, int termMonths);
        Task<bool> DeleteObjectiveAsync(int objectiveId, int studentId);
        Task<CalculationResponseDto?> RunSimulationAsync(int scenarioId, int studentId, LoanParamsDto? loanParams = null, SavingsParamsDto? savingsParams = null, CashFlowParamsDto? cashFlowParams = null);
        Task<List<SimulationHistory>> GetSimulationHistoryAsync(int scenarioId, int studentId);
    }

    public class SimulationService : ISimulationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<SimulationService> _logger;

        public SimulationService(
            ApplicationDbContext context, 
            IHttpClientFactory httpClientFactory,
            ILogger<SimulationService> logger)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<List<Scenario>> GetMyScenariosAsync(int studentId)
        {
            return await _context.Scenarios
                .Include(s => s.Entries)
                .Include(s => s.Histories)
                .Where(s => s.StudentId == studentId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<Scenario?> GetScenarioDetailsAsync(int scenarioId, int studentId)
        {
            return await _context.Scenarios
                .Include(s => s.Entries)
                .Include(s => s.Objectives)
                .Include(s => s.Histories)
                .FirstOrDefaultAsync(s => s.Id == scenarioId && s.StudentId == studentId);
        }

        public async Task<Scenario> CreateScenarioAsync(int studentId, string familyName, float initialBalance)
        {
            var scenario = new Scenario
            {
                StudentId = studentId,
                FamilyName = familyName,
                InitialBalance = initialBalance,
                CreatedAt = DateTime.UtcNow
            };

            _context.Scenarios.Add(scenario);
            await _context.SaveChangesAsync();
            return scenario;
        }

        public async Task<bool> DeleteScenarioAsync(int scenarioId, int studentId)
        {
            var scenario = await _context.Scenarios
                .FirstOrDefaultAsync(s => s.Id == scenarioId && s.StudentId == studentId);
                
            if (scenario == null) return false;

            _context.Scenarios.Remove(scenario);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddEntryAsync(int scenarioId, int studentId, string type, int categoryId, float amount, string month, string recurrence)
        {
            var scenarioExists = await _context.Scenarios.AnyAsync(s => s.Id == scenarioId && s.StudentId == studentId);
            if (!scenarioExists) return false;

            var entryType = await _context.EntryTypes.FirstOrDefaultAsync(t => t.Type.ToUpper() == type.ToUpper());
            if (entryType == null) return false;

            FinancialEntry entry;
            if (type.ToUpper() == "INCOME" || type.ToUpper() == "RENDIMENTO")
            {
                entry = new Income { 
                    ScenarioId = scenarioId, 
                    TypeId = entryType.Id, 
                    CategoryId = categoryId, 
                    Amount = amount, 
                    Month = month, 
                    Recurrence = recurrence 
                };
            }
            else
            {
                entry = new Expense { 
                    ScenarioId = scenarioId, 
                    TypeId = entryType.Id, 
                    CategoryId = categoryId, 
                    Amount = amount, 
                    Month = month, 
                    Recurrence = recurrence 
                };
            }

            _context.FinancialEntries.Add(entry);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateEntryAsync(int entryId, int studentId, int categoryId, float amount, string month, string recurrence)
        {
            var entry = await _context.FinancialEntries
                .Include(e => e.Scenario)
                .FirstOrDefaultAsync(e => e.Id == entryId && e.Scenario!.StudentId == studentId);

            if (entry == null) return false;

            entry.CategoryId = categoryId;
            entry.Amount = amount;
            entry.Month = month;
            entry.Recurrence = recurrence;

            _context.FinancialEntries.Update(entry);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteEntryAsync(int entryId, int studentId)
        {
            var entry = await _context.FinancialEntries
                .Include(e => e.Scenario)
                .FirstOrDefaultAsync(e => e.Id == entryId && e.Scenario!.StudentId == studentId);

            if (entry == null) return false;

            _context.FinancialEntries.Remove(entry);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddObjectiveAsync(int scenarioId, int studentId, string description, float targetValue, int termMonths)
        {
            var scenarioExists = await _context.Scenarios.AnyAsync(s => s.Id == scenarioId && s.StudentId == studentId);
            if (!scenarioExists) return false;

            var objective = new Objective
            {
                ScenarioId = scenarioId,
                Description = description,
                TargetValue = targetValue,
                TargetMonths = termMonths,
                CurrentValue = 0
            };

            _context.Objectives.Add(objective);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteObjectiveAsync(int objectiveId, int studentId)
        {
            var objective = await _context.Objectives
                .Include(o => o.Scenario)
                .FirstOrDefaultAsync(o => o.Id == objectiveId && o.Scenario!.StudentId == studentId);

            if (objective == null) return false;

            _context.Objectives.Remove(objective);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<CalculationResponseDto?> RunSimulationAsync(int scenarioId, int studentId, LoanParamsDto? loanParams = null, SavingsParamsDto? savingsParams = null, CashFlowParamsDto? cashFlowParams = null)
        {
            var scenario = await _context.Scenarios
                .Include(s => s.Entries)
                .Include(s => s.Objectives)
                .FirstOrDefaultAsync(s => s.Id == scenarioId && s.StudentId == studentId);

            if (scenario == null) return null;

            var request = new CalculationRequestDto
            {
                InitialBalance = (decimal)scenario.InitialBalance,
                Entries = scenario.Entries.Select(e => new EntryRequestDto
                {
                    Type = e is Income ? "INCOME" : "EXPENSE",
                    Category = "General", 
                    Amount = (decimal)e.Amount,
                    Month = int.TryParse(e.Month, out int m) ? m : 1,
                    Recurrence = e.Recurrence == "True" || e.Recurrence == "1"
                }).ToList(),
                Objectives = scenario.Objectives.Select(o => new ObjectiveRequestDto
                {
                    Description = o.Description,
                    TargetValue = (decimal)o.TargetValue,
                    TermMonths = o.TargetMonths
                }).ToList(),
                LoanParams = loanParams,
                SavingsParams = savingsParams,
                CashFlowParams = cashFlowParams
            };

            try
            {
                var client = _httpClientFactory.CreateClient("FinancialEngine");
                
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                
                var response = await client.PostAsync("calculate/", content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<CalculationResponseDto>();
                    if (result != null)
                    {
                        var history = new SimulationHistory
                        {
                            ScenarioId = scenarioId,
                            ExecutionDate = DateTime.UtcNow,
                            FinalBalance = result.Projections.BalanceAfter12Months,
                            EffortRate = result.Summary.EffortRatePercentage,
                            MonthsToGoal = result.ObjectivesAnalysis.FirstOrDefault()?.MonthsToGoal ?? 0,
                            ResultsJson = JsonSerializer.Serialize(result)
                        };
                        _context.SimulationHistories.Add(history);
                        await _context.SaveChangesAsync();
                    }
                    return result;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Financial Engine returned error {StatusCode}: {Error}", response.StatusCode, errorContent);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling Financial Engine for Scenario {ScenarioId}", scenarioId);
            }

            return null;
        }

        public async Task<List<SimulationHistory>> GetSimulationHistoryAsync(int scenarioId, int studentId)
        {
            return await _context.SimulationHistories
                .Where(h => h.ScenarioId == scenarioId && h.Scenario!.StudentId == studentId)
                .OrderByDescending(h => h.ExecutionDate)
                .ToListAsync();
        }
    }
}
