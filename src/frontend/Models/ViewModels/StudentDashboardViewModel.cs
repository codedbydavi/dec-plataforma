using Frontend.Models.Entities;
using Frontend.Models.DTOs;

namespace Frontend.Models.ViewModels
{
    public class StudentDashboardViewModel
    {
        public List<Classroom> Classes { get; set; } = new();
        public List<Scenario> Scenarios { get; set; } = new();
        public List<Challenge> Challenges { get; set; } = new();
        public Scenario? SelectedScenario { get; set; }
        public CalculationResponseDto? LatestResult { get; set; }
        

        public JoinClassViewModel JoinClass { get; set; } = new();
        public CreateScenarioViewModel CreateScenario { get; set; } = new();
    }
}
