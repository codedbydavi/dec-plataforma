from decimal import Decimal
import math

class FinancialCalculationService:
    @staticmethod
    def calculate_loan_amortization_schedule(principal, annual_interest_rate, term_months):
        if principal <= 0 or term_months <= 0:
            return [], Decimal('0.00'), Decimal('0.00')

        r = Decimal(str(annual_interest_rate)) / 100 / 12
        n = term_months
        
        if r == 0:
            monthly_installment = principal / n
        else:
            monthly_installment = principal * (r * (1 + r)**n) / ((1 + r)**n - 1)
        
        monthly_installment = monthly_installment.quantize(Decimal('0.01'))
        
        schedule = []
        balance = principal
        for month in range(1, n + 1):
            interest = (balance * r).quantize(Decimal('0.01'))
            principal_payment = monthly_installment - interest
            balance -= principal_payment
            
            schedule.append({
                "month": month,
                "payment": float(monthly_installment),
                "principal": float(principal_payment),
                "interest": float(interest),
                "balance": max(0.0, float(balance))
            })

        total_cost = monthly_installment * n
        return schedule, monthly_installment, total_cost.quantize(Decimal('0.01'))

    @staticmethod
    def calculate_savings_projection(principal, monthly_contribution, annual_interest_rate, term_months):
        r = Decimal(str(annual_interest_rate)) / 100 / 12
        balance = Decimal(str(principal))
        contribution = Decimal(str(monthly_contribution))
        
        schedule = []
        for month in range(1, term_months + 1):
            interest = (balance * r).quantize(Decimal('0.01'))
            balance += interest + contribution
            
            schedule.append({
                "month": month,
                "balance": float(balance - interest - contribution),
                "interest": float(interest),
                "total": float(balance)
            })
            
        return schedule

    @staticmethod
    def calculate_cash_flow_projection(monthly_income, fixed_expenses, variable_expenses, inflation_rate, term_months):
        monthly_inflation = Decimal(str(inflation_rate)) / 100 / 12
        income = Decimal(str(monthly_income))
        fixed = Decimal(str(fixed_expenses))
        variable = Decimal(str(variable_expenses))
        
        schedule = []
        cumulative_savings = Decimal('0.00')
        
        for month in range(1, term_months + 1):
            inflation_factor = (1 + monthly_inflation) ** (month - 1)
            adjusted_variable = variable * inflation_factor
            total_expenses = fixed + adjusted_variable
            net_cash_flow = income - total_expenses
            cumulative_savings += net_cash_flow
            
            schedule.append({
                "month": month,
                "income": float(income),
                "expenses": float(total_expenses),
                "net_cash_flow": float(net_cash_flow),
                "cumulative_savings": float(cumulative_savings)
            })
            
        return schedule

    @staticmethod
    def calculate(initial_balance, entries, objectives, loan_params=None, savings_params=None, cash_flow_params=None):
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
            
            schedule, installment, total_cost = FinancialCalculationService.calculate_loan_amortization_schedule(principal, interest, term)
            
            loan_result = {
                "monthly_installment": float(installment),
                "total_cost": float(total_cost),
                "total_interest": float(total_cost - principal),
                "schedule": schedule
            }
            # Add to monthly expenses for "what-if" impact
            total_monthly_expenses += installment
            
        # Handle Savings Projection if provided
        savings_result = None
        if savings_params:
            monthly_contrib = Decimal(str(savings_params.get('monthly_contribution', 0)))
            interest = Decimal(str(savings_params.get('annual_interest_rate', 0)))
            term = int(savings_params.get('term_months', 0))
            
            schedule = FinancialCalculationService.calculate_savings_projection(initial_balance, monthly_contrib, interest, term)
            
            savings_result = {
                "schedule": schedule,
                "final_amount": float(schedule[-1]['total']) if schedule else float(initial_balance)
            }
            
        # Handle Cash Flow Projection if provided
        cash_flow_result = None
        if cash_flow_params:
            income = Decimal(str(cash_flow_params.get('monthly_income', total_monthly_income)))
            fixed = Decimal(str(cash_flow_params.get('fixed_expenses', total_monthly_expenses)))
            variable = Decimal(str(cash_flow_params.get('variable_expenses', 0)))
            inflation = Decimal(str(cash_flow_params.get('inflation_rate', 0)))
            term = int(cash_flow_params.get('term_months', 0))
            
            schedule = FinancialCalculationService.calculate_cash_flow_projection(income, fixed, variable, inflation, term)
            
            cash_flow_result = {
                "schedule": schedule,
                "cumulative_savings": float(schedule[-1]['cumulative_savings']) if schedule else 0.0
            }

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
            "credit_simulation": loan_result,
            "savings_simulation": savings_result,
            "cash_flow_simulation": cash_flow_result
        }
