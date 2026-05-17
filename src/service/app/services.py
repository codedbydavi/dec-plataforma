class FinancialService:
    """
    Service responsible for financial business logic.
    Follows OOP and Clean Code principles.
    """
    
    def calculate_savings(self, data: dict) -> dict:
        income = data.get('income', 0)
        expenses = data.get('expenses', 0)
        
        # Simple initial logic, expandable in the future
        savings = income - expenses
        
        return {
            "income": income,
            "expenses": expenses,
            "savings": savings,
            "message": "Calculation successfully performed by the financial engine."
        }
