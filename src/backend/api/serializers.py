from rest_framework import serializers
from .models import CalculationLog

class CalculationLogSerializer(serializers.ModelSerializer):
    class Meta:
        model = CalculationLog
        fields = '__all__'

class EntrySerializer(serializers.Serializer):
    type = serializers.ChoiceField(choices=['INCOME', 'EXPENSE'])
    category = serializers.CharField(max_length=100)
    amount = serializers.DecimalField(max_digits=12, decimal_places=2)
    month = serializers.IntegerField(min_value=1, max_value=12)
    recurrence = serializers.BooleanField(default=False)

class ObjectiveSerializer(serializers.Serializer):
    description = serializers.CharField(max_length=255)
    target_value = serializers.DecimalField(max_digits=12, decimal_places=2)
    term_months = serializers.IntegerField(min_value=1)

class LoanParamsSerializer(serializers.Serializer):
    principal = serializers.DecimalField(max_digits=12, decimal_places=2)
    annual_interest_rate = serializers.DecimalField(max_digits=5, decimal_places=2)
    term_months = serializers.IntegerField(min_value=1)

class FinancialDataSerializer(serializers.Serializer):
    """
    Input serializer for financial calculations.
    """
    initial_balance = serializers.DecimalField(max_digits=12, decimal_places=2)
    entries = EntrySerializer(many=True)
    objectives = ObjectiveSerializer(many=True, required=False, default=[])
    loan_params = LoanParamsSerializer(required=False, allow_null=True)
