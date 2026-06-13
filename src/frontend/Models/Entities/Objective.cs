namespace Frontend.Models.Entities
{
    /// <summary>
    /// Savings objective within a scenario.
    /// </summary>
    public class Objective
    {
        private int _id;
        private int _scenarioId;
        private string _description = string.Empty;
        private float _targetValue;
        private float _currentValue;
        private int _targetMonths;

        public int Id { get => _id; set => _id = value; }
        public int ScenarioId { get => _scenarioId; set => _scenarioId = value; }
        public string Description { get => _description; set => _description = value?.Trim() ?? string.Empty; }
        public float TargetValue { get => _targetValue; set => _targetValue = value; }
        public float CurrentValue { get => _currentValue; set => _currentValue = value; }
        public int TargetMonths { get => _targetMonths; set => _targetMonths = value; }

        public virtual Scenario? Scenario { get; set; }

        public Objective()
        {
        }
    }
}
