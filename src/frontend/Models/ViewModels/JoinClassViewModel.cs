using System.ComponentModel.DataAnnotations;

namespace Frontend.Models.ViewModels
{
    public class JoinClassViewModel
    {
        [Required]
        [Display(Name = "Join Code")]
        public string JoinCode { get; set; } = string.Empty;
    }
}
