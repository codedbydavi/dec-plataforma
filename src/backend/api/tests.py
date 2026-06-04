from django.test import TestCase
from decimal import Decimal
from .services import FinancialCalculationService

class FinancialEngineTests(TestCase):
    def setUp(self):
        self.basic_entries = [
            {'type': 'INCOME', 'amount': Decimal('2000.00'), 'category': 'Salary', 'month': 1, 'recurrence': True},
            {'type': 'EXPENSE', 'amount': Decimal('800.00'), 'category': 'Rent', 'month': 1, 'recurrence': True},
            {'type': 'EXPENSE', 'amount': Decimal('200.00'), 'category': 'Food', 'month': 1, 'recurrence': True},
        ]
        self.initial_balance = Decimal('1000.00')

    def test_basic_calculation(self):
        """Test calculation with normal income and expenses."""
        results = FinancialCalculationService.calculate(
            self.initial_balance, self.basic_entries, []
        )
        
        summary = results['summary']
        self.assertEqual(summary['total_monthly_income'], 2000.0)
        self.assertEqual(summary['total_monthly_expenses'], 1000.0)
        self.assertEqual(summary['monthly_balance'], 1000.0)
        self.assertEqual(summary['effort_rate_percentage'], 50.0)
        
        projections = results['projections']
        # 1000 (initial) + 1000 (savings) * 12 = 13000
        self.assertEqual(projections['balance_after_12_months'], 13000.0)
        self.assertFalse(projections['is_sustainable']) # Effort rate 50% > 40%

    def test_zero_income(self):
        """Ensure the engine handles zero income without crashing (effort rate)."""
        entries = [{'type': 'EXPENSE', 'amount': Decimal('500.00'), 'category': 'Misc', 'month': 1, 'recurrence': True}]
        results = FinancialCalculationService.calculate(Decimal('100.00'), entries, [])
        
        self.assertEqual(results['summary']['effort_rate_percentage'], 0.0)
        self.assertEqual(results['summary']['monthly_balance'], -500.0)

    def test_objective_attainability(self):
        """Test if objectives are correctly marked as attainable or not."""
        objectives = [
            {'description': 'New Laptop', 'target_value': Decimal('1500.00'), 'term_months': 1}, # Should be possible (needed 500, savings 1000)
            {'description': 'Car', 'target_value': Decimal('20000.00'), 'term_months': 10}, # Needs 19000, 19 months needed > 10
        ]
        
        results = FinancialCalculationService.calculate(
            self.initial_balance, self.basic_entries, objectives
        )
        
        obj_analysis = results['objectives_analysis']
        self.assertTrue(obj_analysis[0]['is_attainable'])
        self.assertEqual(obj_analysis[0]['months_to_goal'], 1)
        
        self.assertFalse(obj_analysis[1]['is_attainable'])
        self.assertEqual(obj_analysis[1]['months_to_goal'], 19)

    def test_already_reached_objective(self):
        """Test objectives where initial balance is already enough."""
        objectives = [{'description': 'Book', 'target_value': Decimal('50.00'), 'term_months': 1}]
        results = FinancialCalculationService.calculate(Decimal('100.00'), [], objectives)
        
        self.assertTrue(results['objectives_analysis'][0]['is_attainable'])
        self.assertEqual(results['objectives_analysis'][0]['months_to_goal'], 0)

    def test_loan_simulation(self):
        """Test loan installment calculation and its impact on budget."""
        loan_params = {
            'principal': Decimal('10000.00'),
            'annual_interest_rate': Decimal('5.00'),
            'term_months': 24
        }
        
        # Test just the loan formula
        installment, total_cost = FinancialCalculationService.calculate_loan_installment(
            loan_params['principal'], loan_params['annual_interest_rate'], loan_params['term_months']
        )
        
        # Expected PMT = 438.71
        self.assertEqual(installment, Decimal('438.71'))
        self.assertEqual(total_cost, Decimal('438.71') * 24)

        # Test impact on main calculation
        results = FinancialCalculationService.calculate(
            initial_balance=Decimal('1000.00'),
            entries=[{'type': 'INCOME', 'amount': Decimal('2000.00'), 'category': 'Salary', 'month': 1, 'recurrence': True}],
            objectives=[],
            loan_params=loan_params
        )
        
        # Monthly balance should be 2000 - 438.71 = 1561.29
        self.assertEqual(results['summary']['monthly_balance'], 1561.29)
        self.assertIn('credit_simulation', results)
        self.assertEqual(results['credit_simulation']['monthly_installment'], 438.71)
