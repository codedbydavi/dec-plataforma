using System.ComponentModel.DataAnnotations;

namespace Frontend.Models.ViewModels
{
    public class AddObjectiveViewModel
    {
        public int ScenarioId { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0.01, float.MaxValue)]
        public float TargetValue { get; set; }

        [Required]
        [Range(1, 120)]
        public int TargetMonths { get; set; }
    }
}
