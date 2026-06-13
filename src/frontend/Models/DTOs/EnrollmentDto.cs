using System;

namespace Frontend.Models.DTOs
{
    public record EnrollmentDto(int Id, int ClassId, string ClassName, DateTime EnrolledAt);
}
