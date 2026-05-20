namespace Frontend.Models.DTOs
{
    public class ScenarioDto
    {
        public int Id { get; set; }
        public string Family_Name { get; set; } = string.Empty;
        public decimal Initial_Balance { get; set; }
        public DateTime Created_At { get; set; }
        public List<EntryDto> Entries { get; set; } = new();
        public List<ObjectiveDto> Objectives { get; set; } = new();
        public List<SimulationHistoryDto> Histories { get; set; } = new();
    }

    public class EntryDto
    {
        public int Id { get; set; }
        public int Scenario { get; set; }
        public string Type { get; set; } = string.Empty; // INCOME, EXPENSE
        public string Category { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public int Month { get; set; }
        public bool Recurrence { get; set; }
    }

    public class ObjectiveDto
    {
        public int Id { get; set; }
        public int Scenario { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Target_Value { get; set; }
        public int Term_Months { get; set; }
    }

    public class SimulationHistoryDto
    {
        public int Id { get; set; }
        public int Scenario { get; set; }
        public DateTime Execution_Date { get; set; }
        public string Json_Results { get; set; } = string.Empty;
    }
}
