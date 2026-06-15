namespace Frontend.Models.Entities
{




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

        }

        public override void RemoveStudent(Student student)
        {

        }
    }
}
