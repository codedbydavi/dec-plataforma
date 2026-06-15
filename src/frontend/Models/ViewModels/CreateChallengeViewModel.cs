using System.ComponentModel.DataAnnotations;

namespace Frontend.Models.ViewModels
{
    public class CreateChallengeViewModel
    {
        [Required]
        [Display(Name = "Challenge Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Url]
        [Display(Name = "Resource Link (External)")]
        public string AccessLink { get; set; } = string.Empty;

        [Display(Name = "Assign to Class")]
        public int? TargetClassroomId { get; set; }
    }
}
