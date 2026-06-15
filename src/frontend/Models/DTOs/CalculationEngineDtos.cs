using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Frontend.Models.DTOs
{
    public class CalculationRequestDto
    {
        [JsonPropertyName("initial_balance")]
        public decimal InitialBalance { get; set; }

        [JsonPropertyName("entries")]
        public List<EntryRequestDto> Entries { get; set; } = new();

        [JsonPropertyName("objectives")]
        public List<ObjectiveRequestDto> Objectives { get; set; } = new();

        [JsonPropertyName("loan_params")]
        public LoanParamsDto? LoanParams { get; set; }

        [JsonPropertyName("savings_params")]
        public SavingsParamsDto? SavingsParams { get; set; }

        [JsonPropertyName("cash_flow_params")]
        public CashFlowParamsDto? CashFlowParams { get; set; }
    }

    public class LoanParamsDto
    {
        [JsonPropertyName("principal")]
        public decimal Principal { get; set; }

        [JsonPropertyName("annual_interest_rate")]
        public decimal AnnualInterestRate { get; set; }

        [JsonPropertyName("term_months")]
        public int TermMonths { get; set; }
    }

    public class SavingsParamsDto
    {
        [JsonPropertyName("monthly_contribution")]
        public decimal MonthlyContribution { get; set; }

        [JsonPropertyName("annual_interest_rate")]
        public decimal AnnualInterestRate { get; set; }

        [JsonPropertyName("term_months")]
        public int TermMonths { get; set; }
    }

    public class CashFlowParamsDto
    {
        [JsonPropertyName("monthly_income")]
        public decimal MonthlyIncome { get; set; }

        [JsonPropertyName("fixed_expenses")]
        public decimal FixedExpenses { get; set; }

        [JsonPropertyName("variable_expenses")]
        public decimal VariableExpenses { get; set; }

        [JsonPropertyName("inflation_rate")]
        public decimal InflationRate { get; set; }

        [JsonPropertyName("term_months")]
        public int TermMonths { get; set; }
    }

    public class EntryRequestDto
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty; 

        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("month")]
        public int Month { get; set; }

        [JsonPropertyName("recurrence")]
        public bool Recurrence { get; set; }
    }

    public class ObjectiveRequestDto
    {
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("target_value")]
        public decimal TargetValue { get; set; }

        [JsonPropertyName("term_months")]
        public int TermMonths { get; set; }
    }

    public class CalculationResponseDto
    {
        [JsonPropertyName("summary")]
        public CalculationSummaryDto Summary { get; set; } = new();

        [JsonPropertyName("projections")]
        public CalculationProjectionsDto Projections { get; set; } = new();

        [JsonPropertyName("objectives_analysis")]
        public List<ObjectiveAnalysisDto> ObjectivesAnalysis { get; set; } = new();

        [JsonPropertyName("credit_simulation")]
        public CreditSimulationDto? CreditSimulation { get; set; }

        [JsonPropertyName("savings_simulation")]
        public SavingsSimulationDto? SavingsSimulation { get; set; }

        [JsonPropertyName("cash_flow_simulation")]
        public CashFlowSimulationDto? CashFlowSimulation { get; set; }
    }

    public class CreditSimulationDto
    {
        [JsonPropertyName("monthly_installment")]
        public float MonthlyInstallment { get; set; }

        [JsonPropertyName("total_cost")]
        public float TotalCost { get; set; }

        [JsonPropertyName("total_interest")]
        public float TotalInterest { get; set; }

        [JsonPropertyName("schedule")]
        public List<LoanScheduleItemDto> Schedule { get; set; } = new();
    }

    public class LoanScheduleItemDto
    {
        [JsonPropertyName("month")]
        public int Month { get; set; }

        [JsonPropertyName("payment")]
        public float Payment { get; set; }

        [JsonPropertyName("principal")]
        public float Principal { get; set; }

        [JsonPropertyName("interest")]
        public float Interest { get; set; }

        [JsonPropertyName("balance")]
        public float Balance { get; set; }
    }

    public class SavingsSimulationDto
    {
        [JsonPropertyName("final_amount")]
        public float FinalAmount { get; set; }

        [JsonPropertyName("schedule")]
        public List<SavingsScheduleItemDto> Schedule { get; set; } = new();
    }

    public class SavingsScheduleItemDto
    {
        [JsonPropertyName("month")]
        public int Month { get; set; }

        [JsonPropertyName("balance")]
        public float Balance { get; set; }

        [JsonPropertyName("interest")]
        public float Interest { get; set; }

        [JsonPropertyName("total")]
        public float Total { get; set; }
    }

    public class CashFlowSimulationDto
    {
        [JsonPropertyName("cumulative_savings")]
        public float CumulativeSavings { get; set; }

        [JsonPropertyName("schedule")]
        public List<CashFlowScheduleItemDto> Schedule { get; set; } = new();
    }

    public class CashFlowScheduleItemDto
    {
        [JsonPropertyName("month")]
        public int Month { get; set; }

        [JsonPropertyName("income")]
        public float Income { get; set; }

        [JsonPropertyName("expenses")]
        public float Expenses { get; set; }

        [JsonPropertyName("net_cash_flow")]
        public float NetCashFlow { get; set; }

        [JsonPropertyName("cumulative_savings")]
        public float CumulativeSavings { get; set; }
    }

    public class CalculationSummaryDto
    {
        [JsonPropertyName("total_monthly_income")]
        public float TotalMonthlyIncome { get; set; }

        [JsonPropertyName("total_monthly_expenses")]
        public float TotalMonthlyExpenses { get; set; }

        [JsonPropertyName("monthly_balance")]
        public float MonthlyBalance { get; set; }

        [JsonPropertyName("effort_rate_percentage")]
        public float EffortRatePercentage { get; set; }
    }

    public class CalculationProjectionsDto
    {
        [JsonPropertyName("balance_after_12_months")]
        public float BalanceAfter12Months { get; set; }

        [JsonPropertyName("is_sustainable")]
        public bool IsSustainable { get; set; }
    }

    public class ObjectiveAnalysisDto
    {
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("target_value")]
        public float TargetValue { get; set; }

        [JsonPropertyName("months_to_goal")]
        public int MonthsToGoal { get; set; }

        [JsonPropertyName("is_attainable")]
        public bool IsAttainable { get; set; }
    }
}
