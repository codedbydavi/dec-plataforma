namespace Frontend.Models.Entities
{
    /// <summary>
    /// Negative financial entry.
    /// Following the Inheritance pillar.
    /// </summary>
    public class Expense : FinancialEntry
    {
        private bool _isEssential;

        public bool IsEssential
        {
            get => _isEssential;
            set => _isEssential = value;
        }

        public Expense() : base()
        {
        }
    }
}
