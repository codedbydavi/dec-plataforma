using System;
using System.Collections.Generic;

namespace Frontend.Models.Entities
{
    /// <summary>
    /// Financial Scenario created by a student.
    /// Following Encapsulation and Abstraction.
    /// </summary>
    public class Scenario
    {
        private int _id;
        private int _studentId;
        private string _familyName = string.Empty;
        private float _initialBalance;
        private readonly List<FinancialEntry> _entries = new();
        private readonly List<Objective> _objectives = new();

        public int Id { get => _id; set => _id = value; }
        public int StudentId { get => _studentId; set => _studentId = value; }
        public string FamilyName { get => _familyName; set => _familyName = value?.Trim() ?? string.Empty; }
        public float InitialBalance { get => _initialBalance; set => _initialBalance = value; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual Student? Student { get; set; }
        public virtual IReadOnlyCollection<FinancialEntry> Entries => _entries.AsReadOnly();
        public virtual IReadOnlyCollection<Objective> Objectives => _objectives.AsReadOnly();
        public virtual ICollection<SimulationHistory> Histories { get; set; } = new List<SimulationHistory>();

        public Scenario()
        {
        }

        public void AddEntry(FinancialEntry entry)
        {
            if (entry == null) return;
            _entries.Add(entry);
        }

        public void AddObjective(Objective objective)
        {
            if (objective == null) return;
            _objectives.Add(objective);
        }
    }
}
