namespace Frontend.Models.DTOs
{
    public record SimulationResultDto(
        float TotalIncome, 
        float TotalExpenses, 
        float MonthlyBalance, 
        float EffortRate, 
        float Projection12Months,
        bool IsSustainable
    );
}
