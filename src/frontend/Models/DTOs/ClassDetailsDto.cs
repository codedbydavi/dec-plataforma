using System.Collections.Generic;

namespace Frontend.Models.DTOs
{
    public record ClassDetailsDto(int Id, string Name, int MemberCode, string TeacherName, List<StudentDto> Students) 
        : ClassGroupDto(Id, Name, MemberCode, TeacherName, Students.Count);
}
