/**
 * Financial calculation utilities for DEC platform
 */

export interface SavingsProjection {
  month: number;
  balance: number;
  interest: number;
  total: number;
}

export interface LoanAmortization {
  month: number;
  payment: number;
  principal: number;
  interest: number;
  balance: number;
}

/**
 * Calculate compound interest projection
 * @param principal Initial amount
 * @param monthlyContribution Monthly savings contribution
 * @param annualRate Annual interest rate (e.g., 0.05 for 5%)
 * @param months Number of months
 * @returns Array of monthly projections
 */
export function calculateSavingsProjection(
  principal: number,
  monthlyContribution: number,
  annualRate: number,
  months: number
): SavingsProjection[] {
  const monthlyRate = annualRate / 12;
  const projections: SavingsProjection[] = [];
  let balance = principal;

  for (let month = 1; month <= months; month++) {
    const interest = balance * monthlyRate;
    balance += interest + monthlyContribution;
    
    projections.push({
      month,
      balance: balance - interest - monthlyContribution,
      interest,
      total: balance,
    });
  }

  return projections;
}

/**
 * Calculate loan amortization using French method (Price system)
 * @param principal Loan amount
 * @param annualRate Annual interest rate (e.g., 0.05 for 5%)
 * @param months Number of months
 * @returns Array of monthly amortization schedule
 */
export function calculateLoanAmortization(
  principal: number,
  annualRate: number,
  months: number
): LoanAmortization[] {
  const monthlyRate = annualRate / 12;
  const payment = principal * (monthlyRate * Math.pow(1 + monthlyRate, months)) / 
                  (Math.pow(1 + monthlyRate, months) - 1);
  
  const schedule: LoanAmortization[] = [];
  let balance = principal;

  for (let month = 1; month <= months; month++) {
    const interest = balance * monthlyRate;
    const principalPayment = payment - interest;
    balance -= principalPayment;

    schedule.push({
      month,
      payment,
      principal: principalPayment,
      interest,
      balance: Math.max(0, balance),
    });
  }

  return schedule;
}

/**
 * Calculate effort rate (percentage of income used for fixed expenses)
 * @param monthlyIncome Total monthly income
 * @param fixedExpenses Total fixed expenses
 * @returns Effort rate as percentage
 */
export function calculateEffortRate(monthlyIncome: number, fixedExpenses: number): number {
  if (monthlyIncome === 0) return 0;
  return (fixedExpenses / monthlyIncome) * 100;
}

/**
 * Calculate months needed to reach savings goal
 * @param currentSavings Current savings amount
 * @param goal Target savings goal
 * @param monthlyContribution Monthly savings contribution
 * @param annualRate Annual interest rate (e.g., 0.05 for 5%)
 * @returns Number of months needed
 */
export function calculateMonthsToGoal(
  currentSavings: number,
  goal: number,
  monthlyContribution: number,
  annualRate: number = 0
): number {
  if (monthlyContribution <= 0) return Infinity;
  
  const monthlyRate = annualRate / 12;
  let balance = currentSavings;
  let months = 0;

  while (balance < goal && months < 1000) {
    balance += balance * monthlyRate + monthlyContribution;
    months++;
  }

  return months;
}

/**
 * Calculate cash flow projection with inflation
 * @param monthlyIncome Monthly income
 * @param fixedExpenses Fixed monthly expenses
 * @param variableExpenses Variable monthly expenses
 * @param inflationRate Annual inflation rate (e.g., 0.03 for 3%)
 * @param months Number of months to project
 * @returns Array of monthly cash flow
 */
export function calculateCashFlowProjection(
  monthlyIncome: number,
  fixedExpenses: number,
  variableExpenses: number,
  inflationRate: number,
  months: number
): { month: number; income: number; expenses: number; netCashFlow: number; cumulativeSavings: number }[] {
  const monthlyInflation = inflationRate / 12;
  const projection = [];
  let cumulativeSavings = 0;

  for (let month = 1; month <= months; month++) {
    const inflationFactor = Math.pow(1 + monthlyInflation, month - 1);
    const adjustedVariableExpenses = variableExpenses * inflationFactor;
    const totalExpenses = fixedExpenses + adjustedVariableExpenses;
    const netCashFlow = monthlyIncome - totalExpenses;
    cumulativeSavings += netCashFlow;

    projection.push({
      month,
      income: monthlyIncome,
      expenses: totalExpenses,
      netCashFlow,
      cumulativeSavings,
    });
  }

  return projection;
}

/**
 * Calculate break-even point
 * @param initialCost Initial investment or cost
 * @param monthlyProfit Monthly profit or savings
 * @returns Number of months to break even
 */
export function calculateBreakEvenPoint(initialCost: number, monthlyProfit: number): number {
  if (monthlyProfit <= 0) return Infinity;
  return Math.ceil(initialCost / monthlyProfit);
}

/**
 * Calculate total credit cost
 * @param principal Loan amount
 * @param annualRate Annual interest rate
 * @param months Number of months
 * @returns Total cost including principal and interest
 */
export function calculateTotalCreditCost(
  principal: number,
  annualRate: number,
  months: number
): { totalPaid: number; totalInterest: number; totalCost: number } {
  const schedule = calculateLoanAmortization(principal, annualRate, months);
  const totalPaid = schedule.reduce((sum, payment) => sum + payment.payment, 0);
  const totalInterest = totalPaid - principal;

  return {
    totalPaid,
    totalInterest,
    totalCost: totalPaid,
  };
}
