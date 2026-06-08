using System.ComponentModel.DataAnnotations;

namespace Frontend.Models.ViewModels
{
    public class AddEntryViewModel
    {
        public int ScenarioId { get; set; }

        [Required]
        public int TypeId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [Range(0.01, float.MaxValue)]
        public float Amount { get; set; }

        [Required]
        public string Month { get; set; } = string.Empty;

        public bool Recurrence { get; set; }
    }
}
