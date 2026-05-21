using System.Collections.Generic;
using System.Linq;

namespace Frontend.Models.Entities
{
    /// <summary>
    /// Concrete implementation of a classroom.
    /// Following the Inheritance pillar.
    /// </summary>
    public class Classroom : LearningActivity
    {
        private int _memberCode;
        private int _teacherId;

        public int MemberCode
        {
            get => _memberCode;
            set => _memberCode = value;
        }

        public int TeacherId
        {
            get => _teacherId;
            set => _teacherId = value;
        }

        public virtual Professor? Teacher { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();


        public IEnumerable<Student> Students => Enrollments.Select(e => e.Student).Where(s => s != null)!;

        public override void AddStudent(Student student)
        {
            // Logic to handle enrollment
        }

        public override void RemoveStudent(Student student)
        {
            // Logic to remove enrollment
        }
    }
}
