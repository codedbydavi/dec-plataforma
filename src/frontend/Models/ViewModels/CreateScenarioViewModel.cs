using System.ComponentModel.DataAnnotations;

namespace Frontend.Models.ViewModels
{
    public class CreateScenarioViewModel
    {
        [Required]
        [Display(Name = "Family Name")]
        public string FamilyName { get; set; } = string.Empty;

        [Required]
        [Range(0, float.MaxValue)]
        [Display(Name = "Initial Balance")]
        public float InitialBalance { get; set; }
    }
}
