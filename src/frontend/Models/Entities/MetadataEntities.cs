using System;

namespace Frontend.Models.Entities
{
    // Relationship / Join Entities
    public class Enrollment
    {
        public int Id { get; set; }
        public int ClassGroupId { get; set; }
        public int StudentId { get; set; }
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;

        public virtual Classroom? ClassGroup { get; set; }
        public virtual Student? Student { get; set; }
    }

    public class SimulationHistory
    {
        public int Id { get; set; }
        public int ScenarioId { get; set; }
        public DateTime ExecutionDate { get; set; } = DateTime.UtcNow;
        public float FinalBalance { get; set; }
        public int MonthsToGoal { get; set; }
        public float EffortRate { get; set; }
        public string ResultsJson { get; set; } = "{}";

        // Evaluation fields (US_C015)
        public float? Score { get; set; }
        public string? Feedback { get; set; }

        public virtual Scenario? Scenario { get; set; }
    }

    public class ChallengeAssignment
    {
        public int Id { get; set; }
        public int ChallengeId { get; set; }
        public int ClassroomId { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

        public virtual Challenge? Challenge { get; set; }
        public virtual Classroom? Classroom { get; set; }
    }

    // Lookup Entities (MER alignment)
    public class RoleLookup
    {
        public int Id { get; set; }
        public string Role { get; set; } = string.Empty;
    }

    public class UserStatus
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class ClassStatus
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class Gender
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class EntryType
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
