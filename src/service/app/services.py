class FinancialService:
    """
    Serviço responsável pela lógica de negócio financeira.
    Segue princípios de POO e Clean Code.
    """
    
    def calculate_savings(self, data: dict) -> dict:
        income = data.get('income', 0)
        expenses = data.get('expenses', 0)
        
        # Lógica simples inicial, expansível futuramente
        savings = income - expenses
        
        return {
            "income": income,
            "expenses": expenses,
            "savings": savings,
            "message": "Cálculo efetuado com sucesso pelo motor financeiro."
        }
