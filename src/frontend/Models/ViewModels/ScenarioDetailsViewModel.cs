using Frontend.Models.Entities;
using Frontend.Models.DTOs;

namespace Frontend.Models.ViewModels
{
    public class ScenarioDetailsViewModel
    {
        public Scenario Scenario { get; set; } = null!;
        public List<EntryType> EntryTypes { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
        public CalculationResponseDto? LatestResult { get; set; }
        public DateTime? LastRunDate { get; set; }

        // Form Models for modals
        public AddEntryViewModel AddEntry { get; set; } = new();
        public AddObjectiveViewModel AddObjective { get; set; } = new();

        public LoanParamsDto LoanSimulation { get; set; } = new();
        public SavingsParamsDto SavingsSimulation { get; set; } = new();
        public CashFlowParamsDto CashFlowSimulation { get; set; } = new();
        }
        }

