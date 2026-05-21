using System;

namespace Frontend.Models.DTOs
{
    public record ScenarioDto(int Id, string FamilyName, float InitialBalance, DateTime CreatedAt);
}
