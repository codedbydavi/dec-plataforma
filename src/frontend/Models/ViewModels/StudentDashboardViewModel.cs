using Frontend.Models.Entities;

namespace Frontend.Models.ViewModels
{
    public class StudentDashboardViewModel
    {
        public List<Classroom> Classes { get; set; } = new();
        public List<Scenario> Scenarios { get; set; } = new();
        public List<Challenge> Challenges { get; set; } = new();
        
        // Form Models for modals
        public JoinClassViewModel JoinClass { get; set; } = new();
        public CreateScenarioViewModel CreateScenario { get; set; } = new();
    }
}
