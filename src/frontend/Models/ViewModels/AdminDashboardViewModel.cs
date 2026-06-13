using System.Collections.Generic;

namespace Frontend.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public string[] Labels { get; set; } = new string[7];
        public int[] ScenariosData { get; set; } = new int[7];
        public int[] SimulationsData { get; set; } = new int[7];
        
        // General stats
        public int TotalUsers { get; set; }
        public int TotalScenarios { get; set; }
        public int TotalSimulations { get; set; }
        public int ActiveClasses { get; set; }
    }
}
