namespace Frontend.Models.Entities
{




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
