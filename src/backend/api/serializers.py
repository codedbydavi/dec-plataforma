from rest_framework import serializers
from .models import CalculationLog

class CalculationLogSerializer(serializers.ModelSerializer):
    class Meta:
        model = CalculationLog
        fields = '__all__'

class FinancialDataSerializer(serializers.Serializer):
    """
    Input serializer for financial calculations.
    """
    initial_balance = serializers.DecimalField(max_digits=12, decimal_places=2)
    entries = serializers.ListField(
        child=serializers.DictField()
    )
    objectives = serializers.ListField(
        child=serializers.DictField(),
        required=False
    )
