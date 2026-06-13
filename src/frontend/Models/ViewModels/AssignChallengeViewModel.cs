using System.ComponentModel.DataAnnotations;

namespace Frontend.Models.ViewModels
{
    public class AssignChallengeViewModel
    {
        [Required]
        public int ChallengeId { get; set; }

        [Required]
        public int ClassroomId { get; set; }
    }
}
