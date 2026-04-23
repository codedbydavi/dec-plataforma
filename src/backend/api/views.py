from rest_framework import viewsets
from .models import Simulacao
from .serializers import SimulacaoSerializer

class SimulacaoViewSet(viewsets.ModelViewSet):
    queryset = Simulacao.objects.all()
    serializer_class = SimulacaoSerializer
