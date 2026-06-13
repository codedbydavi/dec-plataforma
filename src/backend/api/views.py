from rest_framework import status
from rest_framework.response import Response
from rest_framework.decorators import api_view, permission_classes
from rest_framework.permissions import AllowAny
from .serializers import FinancialDataSerializer
from .services import FinancialCalculationService
from .models import CalculationLog

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
    
    # Delegate to the service layer
    results = FinancialCalculationService.calculate(
        initial_balance=data['initial_balance'],
        entries=data['entries'],
        objectives=data.get('objectives', []),
        loan_params=data.get('loan_params')
    )

    # Log the calculation for auditing/debugging
    CalculationLog.objects.create(
        request_data=request.data,
        response_data=results
    )

    return Response(results)

@api_view(['GET'])
@permission_classes([AllowAny])
def health_check(request):
    return Response({"status": "healthy", "service": "Financial Calculation Engine"})
