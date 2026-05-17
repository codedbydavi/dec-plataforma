from rest_framework import viewsets, permissions
from rest_framework_simplejwt.views import TokenObtainPairView
from .models import CustomUser, Simulacao
from .serializers import CustomTokenObtainPairSerializer, UserSerializer, SimulacaoSerializer

class CustomTokenObtainPairView(TokenObtainPairView):
    serializer_class = CustomTokenObtainPairSerializer

class SimulacaoViewSet(viewsets.ModelViewSet):
    serializer_class = SimulacaoSerializer
    permission_classes = [permissions.IsAuthenticated]

    def get_queryset(self):
        # Admin can see all, others only their own
        if self.request.user.role == 'ADMIN':
            return Simulacao.objects.all()
        return Simulacao.objects.filter(user=self.request.user)

    def perform_create(self, serializer):
        serializer.save(user=self.request.user)
