from rest_framework import viewsets, status
from rest_framework.response import Response
from rest_framework.decorators import api_view, permission_classes
from rest_framework.permissions import AllowAny # Will be secured later
from .serializers import FinancialDataSerializer, CalculationLogSerializer
from .models import CalculationLog
import decimal

@api_view(['POST'])
@permission_classes([AllowAny])
def calculate_financials(request):
    """
    Main calculation engine endpoint.
    Receives raw financial data and returns computed indicators.
    """
    serializer = FinancialDataSerializer(data=request.data)
    if not serializer.is_valid():
        return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)

    data = serializer.validated_data
    initial_balance = data['initial_balance']
    entries = data['entries']
    
    # Simple logic to simulate calculations (effort rate, savings projection)
    total_income = decimal.Decimal(0)
    total_expenses = decimal.Decimal(0)
    
    for entry in entries:
        amount = decimal.Decimal(str(entry.get('amount', 0)))
        if entry.get('type') == 'INCOME':
            total_income += amount
        else:
            total_expenses += amount
            
    effort_rate = (total_expenses / total_income * 100) if total_income > 0 else 100
    monthly_savings = total_income - total_expenses
    projection_12_months = initial_balance + (monthly_savings * 12)

    results = {
        "summary": {
            "total_monthly_income": float(total_income),
            "total_monthly_expenses": float(total_expenses),
            "monthly_balance": float(monthly_savings),
            "effort_rate_percentage": float(effort_rate)
        },
        "projections": {
            "balance_after_12_months": float(projection_12_months),
            "is_sustainable": effort_rate < 50
        }
    }

    # Log the calculation
    CalculationLog.objects.create(
        request_data=request.data,
        response_data=results
    )

    return Response(results)

@api_view(['GET'])
@permission_classes([AllowAny])
def health_check(request):
    return Response({"status": "healthy", "service": "Calculation Engine"})
