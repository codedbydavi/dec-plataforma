namespace Frontend.Models.Entities
{
    /// <summary>
    /// Concrete implementation of a challenge.
    /// Following the Inheritance pillar.
    /// </summary>
    public class Challenge : LearningActivity
    {
        private string _accessLink = string.Empty;

        public string AccessLink
        {
            get => _accessLink;
            set => _accessLink = string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
        }

        public override void AddStudent(Student student)
        {
            // Challenge specific enrollment logic
        }

        public override void RemoveStudent(Student student)
        {
            // Challenge specific removal logic
        }
    }
}
