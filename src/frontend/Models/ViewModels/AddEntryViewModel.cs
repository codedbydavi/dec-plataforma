using System.ComponentModel.DataAnnotations;
using Frontend.Models.Entities;
using System.Collections.Generic;

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

        public IEnumerable<EntryType> EntryTypes { get; set; } = new List<EntryType>();
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    }
}

