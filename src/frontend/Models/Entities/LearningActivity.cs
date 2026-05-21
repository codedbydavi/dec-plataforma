using System;

namespace Frontend.Models.Entities
{
    /// <summary>
    /// Abstract base for educational activities.
    /// Following the Abstraction and Polymorphism pillars.
    /// </summary>
    public abstract class LearningActivity
    {
        private int _id;
        private string _name = string.Empty;
        private int _statusId;

        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public string Name
        {
            get => _name;
            set => _name = string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
        }

        public int StatusId
        {
            get => _statusId;
            set => _statusId = value;
        }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ClassStatus? Status { get; set; }

        protected LearningActivity()
        {
        }

        // Polymorphism: Derived classes can define how students are added
        public abstract void AddStudent(Student student);
        public abstract void RemoveStudent(Student student);
    }
}
