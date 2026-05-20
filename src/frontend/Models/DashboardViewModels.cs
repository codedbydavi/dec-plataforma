using System.ComponentModel.DataAnnotations;

namespace Frontend.Models
{
    public class CreateClassViewModel
    {
        [Required(ErrorMessage = "Class name is required")]
        [Display(Name = "Class Name")]
        public string Name { get; set; } = string.Empty;
    }

    public class JoinClassViewModel
    {
        [Required(ErrorMessage = "Join code is required")]
        [Display(Name = "Class Code (e.g., DEC-XXXXXX)")]
        public string JoinCode { get; set; } = string.Empty;
    }

    public class CreateScenarioViewModel
    {
        [Required(ErrorMessage = "Family name is required")]
        [Display(Name = "Family Name")]
        public string FamilyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Initial balance is required")]
        [Display(Name = "Initial Balance")]
        [Range(0, 1000000, ErrorMessage = "Initial balance must be between 0 and 1,000,000")]
        public decimal InitialBalance { get; set; }
    }

    public class AddEntryViewModel
    {
        [Required]
        public int ScenarioId { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public string Type { get; set; } = "EXPENSE";

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, 1000000)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Month is required")]
        [Range(1, 12)]
        public int Month { get; set; } = DateTime.Now.Month;

        public bool Recurrence { get; set; }
    }

    public class AddObjectiveViewModel
    {
        [Required]
        public int ScenarioId { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Target value is required")]
        [Range(1, 10000000)]
        public decimal TargetValue { get; set; }

        [Required(ErrorMessage = "Term in months is required")]
        [Range(1, 120)]
        public int TermMonths { get; set; }
    }
}
