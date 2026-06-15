using System;
using System.Collections.Generic;

namespace Frontend.Models.Entities
{




    public class Scenario
    {
        private int _id;
        private int _studentId;
        private int? _challengeId;
        private string _familyName = string.Empty;
        private float _initialBalance;
        private DateTime _createdAt = DateTime.UtcNow;

        private readonly List<FinancialEntry> _entries = new();
        private readonly List<Objective> _objectives = new();
        private ICollection<SimulationHistory> _histories = new List<SimulationHistory>();

        public int Id { get => _id; set => _id = value; }
        public int StudentId { get => _studentId; set => _studentId = value; }
        public int? ChallengeId { get => _challengeId; set => _challengeId = value; }
        public string FamilyName { get => _familyName; set => _familyName = value?.Trim() ?? string.Empty; }
        public float InitialBalance { get => _initialBalance; set => _initialBalance = value; }
        public DateTime CreatedAt { get => _createdAt; set => _createdAt = value; }

        public virtual Student? Student { get; set; }
        public virtual Challenge? Challenge { get; set; }
        
        public virtual IReadOnlyCollection<FinancialEntry> Entries => _entries.AsReadOnly();
        public virtual IReadOnlyCollection<Objective> Objectives => _objectives.AsReadOnly();
        public virtual ICollection<SimulationHistory> Histories { get => _histories; set => _histories = value; }

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
