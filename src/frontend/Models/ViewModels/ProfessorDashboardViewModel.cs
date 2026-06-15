using Frontend.Models.Entities;

namespace Frontend.Models.ViewModels
{
    public class ProfessorDashboardViewModel
    {
        public List<Classroom> Classes { get; set; } = new();
        public List<Challenge> GlobalChallenges { get; set; } = new();

        // Chart Data
        public List<string> StudentNames { get; set; } = new();
        public List<float> StudentScores { get; set; } = new();
        public List<int> StudentChallenges { get; set; } = new();
        
        public int TotalEvaluated { get; set; }
        public int TotalPending { get; set; }

        public int GlobalAvgCompletionRate { get; set; }
        public float GlobalAvgScore { get; set; }

        // Forms
        public CreateChallengeViewModel CreateChallenge { get; set; } = new();
        public AssignChallengeViewModel AssignChallenge { get; set; } = new();
    }
}
