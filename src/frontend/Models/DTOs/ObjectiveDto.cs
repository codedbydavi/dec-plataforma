namespace Frontend.Models.DTOs
{
    public record ObjectiveDto(int Id, string Description, float TargetValue, float CurrentValue, int TargetMonths);
}
