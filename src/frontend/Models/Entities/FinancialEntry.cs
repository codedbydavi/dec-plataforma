namespace Frontend.Models.Entities
{
    /// <summary>
    /// Abstract base for financial movements.
    /// Following the Abstraction pillar.
    /// </summary>
    public abstract class FinancialEntry
    {
        private int _id;
        private int _scenarioId;
        private int _typeId;
        private int _categoryId;
        private float _amount;
        private string _month = string.Empty;
        private string _recurrence = string.Empty;

        public int Id { get => _id; set => _id = value; }
        public int ScenarioId { get => _scenarioId; set => _scenarioId = value; }
        public int TypeId { get => _typeId; set => _typeId = value; }
        public int CategoryId { get => _categoryId; set => _categoryId = value; }
        public float Amount { get => _amount; set => _amount = value; }
        public string Month { get => _month; set => _month = value; }
        public string Recurrence { get => _recurrence; set => _recurrence = value; }

        public virtual Scenario? Scenario { get; set; }
        public virtual EntryType? EntryType { get; set; }
        public virtual Category? Category { get; set; }
    }
}
