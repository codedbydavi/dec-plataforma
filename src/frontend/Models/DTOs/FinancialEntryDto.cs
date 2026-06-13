namespace Frontend.Models.DTOs
{
    public record FinancialEntryDto(int Id, string Type, string Category, float Amount, string Month, string Recurrence);
}
