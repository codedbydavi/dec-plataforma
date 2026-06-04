from decimal import Decimal
import math

class FinancialCalculationService:
    @staticmethod
    def calculate_loan_installment(principal, annual_interest_rate, term_months):
        """
        Calculates monthly installment and total cost of a loan using the Price system (French amortization).
        Formula: PMT = P * [r(1+r)^n] / [(1+r)^n - 1]
        """
        if principal <= 0 or term_months <= 0:
            return Decimal('0.00'), Decimal('0.00')
        
        # Monthly interest rate
        r = Decimal(str(annual_interest_rate)) / 100 / 12
        n = term_months
        
        if r == 0:
            monthly_installment = principal / n
        else:
            monthly_installment = principal * (r * (1 + r)**n) / ((1 + r)**n - 1)
        
        # Round the installment to 2 decimal places as it's what will be paid
        monthly_installment = monthly_installment.quantize(Decimal('0.01'))
        total_cost = monthly_installment * n
        
        return monthly_installment, total_cost.quantize(Decimal('0.01'))

    @staticmethod
    def calculate(initial_balance, entries, objectives, loan_params=None):
        """
        Core calculation logic for the financial engine.
        Returns a dictionary with calculated indicators and projections.
        """
        total_monthly_income = Decimal('0.00')
        total_monthly_expenses = Decimal('0.00')

        for entry in entries:
            amount = Decimal(str(entry['amount']))
            if entry['type'] == 'INCOME':
                total_monthly_income += amount
            else:
                total_monthly_expenses += amount

        # Handle Loan Impact if provided
        loan_result = None
        if loan_params:
            principal = Decimal(str(loan_params.get('principal', 0)))
            interest = Decimal(str(loan_params.get('annual_interest_rate', 0)))
            term = int(loan_params.get('term_months', 0))
            
            installment, total_cost = FinancialCalculationService.calculate_loan_installment(principal, interest, term)
            
            loan_result = {
                "monthly_installment": float(installment),
                "total_cost": float(total_cost),
                "total_interest": float(total_cost - principal)
            }
            # Add to monthly expenses for "what-if" impact
            total_monthly_expenses += installment

        # 1. Monthly Balance
        monthly_balance = total_monthly_income - total_monthly_expenses

        # 2. Effort Rate (Taxa de Esforço)
        effort_rate = Decimal('0.00')
        if total_monthly_income > 0:
            effort_rate = (total_monthly_expenses / total_monthly_income) * 100

        # 3. 12-Month Projection
        projection_12_months = initial_balance + (monthly_balance * 12)

        # 4. Sustainability check
        is_sustainable = effort_rate <= 40 and monthly_balance > 0

        # 5. Objectives Analysis
        objective_results = []
        for obj in objectives:
            target = Decimal(str(obj['target_value']))
            needed = target - initial_balance
            
            months_to_goal = -1
            
            if needed <= 0:
                months_to_goal = 0
            elif monthly_balance > 0:
                months_to_goal = math.ceil(needed / monthly_balance)
            
            objective_results.append({
                "description": obj['description'],
                "target_value": float(target),
                "months_to_goal": months_to_goal,
                "is_attainable": months_to_goal != -1 and months_to_goal <= obj['term_months']
            })

        return {
            "summary": {
                "total_monthly_income": float(total_monthly_income),
                "total_monthly_expenses": float(total_monthly_expenses),
                "monthly_balance": float(monthly_balance),
                "effort_rate_percentage": round(float(effort_rate), 2)
            },
            "projections": {
                "balance_after_12_months": float(projection_12_months),
                "is_sustainable": is_sustainable
            },
            "objectives_analysis": objective_results,
            "credit_simulation": loan_result
        }
