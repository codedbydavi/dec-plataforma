using Frontend.Models.Entities;

namespace Frontend.Models.ViewModels
{
    public class ProfessorDashboardViewModel
    {
        public List<Classroom> Classes { get; set; } = new();
        public List<Challenge> GlobalChallenges { get; set; } = new();

        // Forms
        public CreateChallengeViewModel CreateChallenge { get; set; } = new();
        public AssignChallengeViewModel AssignChallenge { get; set; } = new();
    }
}
