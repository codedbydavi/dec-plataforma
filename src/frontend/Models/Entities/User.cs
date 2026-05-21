using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Frontend.Models.Entities
{
    /// <summary>
    /// Base class for all users, inheriting from IdentityUser with integer keys.
    /// Following the Abstraction pillar.
    /// </summary>
    public abstract class User : IdentityUser<int>
    {
        private string _fullName = string.Empty;
        private int _userStatusId;
        private int _roleId;
        private int _genderId;
        private DateTime? _birthDate;
        private string? _imgUrl;

        // Encapsulation: Properties with validation or logic
        public string FullName
        {
            get => _fullName;
            set => _fullName = string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
        }

        public int UserStatusId
        {
            get => _userStatusId;
            set => _userStatusId = value;
        }

        public int RoleId
        {
            get => _roleId;
            set => _roleId = value;
        }

        public int GenderId
        {
            get => _genderId;
            set => _genderId = value;
        }

        public DateTime? BirthDate
        {
            get => _birthDate;
            set => _birthDate = value;
        }

        public string? ImgUrl
        {
            get => _imgUrl;
            set => _imgUrl = value;
        }

        // Navigation Properties
        public virtual UserStatus? UserStatus { get; set; }
        public virtual RoleLookup? Role { get; set; }
        public virtual Gender? Gender { get; set; }

        protected User() : base()
        {
        }
    }

    /// <summary>
    /// Concrete implementation for ASP.NET Identity.
    /// </summary>
    public class ApplicationUser : User { }

    /// <summary>
    /// Student specific user.
    /// </summary>
    public class Student : ApplicationUser
    {
        private readonly List<Enrollment> _enrollments = new();
        private readonly List<Scenario> _scenarios = new();

        public virtual IReadOnlyCollection<Enrollment> Enrollments => _enrollments.AsReadOnly();
        public virtual IReadOnlyCollection<Scenario> Scenarios => _scenarios.AsReadOnly();

        public Student() : base() { }

        public void JoinActivity(LearningActivity activity) => activity.AddStudent(this);
    }

    /// <summary>
    /// Professor specific user.
    /// </summary>
    public class Professor : ApplicationUser
    {
        private readonly List<Classroom> _classrooms = new();
        public virtual IReadOnlyCollection<Classroom> Classrooms => _classrooms.AsReadOnly();

        public Professor() : base() { }

        public Classroom CreateClassroom(string name, int memberCode)
        {
            var classroom = new Classroom { Name = name, MemberCode = memberCode, TeacherId = this.Id };
            _classrooms.Add(classroom);
            return classroom;
        }
    }
}
