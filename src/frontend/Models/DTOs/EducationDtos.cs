namespace Frontend.Models.DTOs
{
    public class ClassGroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Teacher_Name { get; set; } = string.Empty;
        public string Join_Code { get; set; } = string.Empty;
        public int Student_Count { get; set; }
        public DateTime Created_At { get; set; }
        public int? Class_Status { get; set; }
    }

    public class StudentDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class ClassDetailsDto : ClassGroupDto
    {
        public List<StudentDto> Students { get; set; } = new();
    }

    public class EnrollmentDto
    {
        public int Id { get; set; }
        public int Class_Group { get; set; }
        public string Class_Name { get; set; } = string.Empty;
        public DateTime Enrolled_At { get; set; }
    }
}
