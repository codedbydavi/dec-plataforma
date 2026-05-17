from rest_framework import viewsets, permissions, status
from rest_framework.response import Response
from rest_framework_simplejwt.views import TokenObtainPairView
from .models import CustomUser, Simulacao, Turma, Inscricao
from .serializers import (
    CustomTokenObtainPairSerializer, 
    UserSerializer, 
    SimulacaoSerializer, 
    TurmaSerializer, 
    InscricaoSerializer
)
from .permissions import IsAdmin, IsTeacher, IsStudent

class CustomTokenObtainPairView(TokenObtainPairView):
    serializer_class = CustomTokenObtainPairSerializer

class TurmaViewSet(viewsets.ModelViewSet):
    serializer_class = TurmaSerializer
    
    def get_permissions(self):
        if self.action in ['create', 'update', 'partial_update', 'destroy']:
            return [(IsTeacher | IsAdmin)()]
        return [permissions.IsAuthenticated()]

    def get_queryset(self):
        user = self.request.user
        if not user.is_authenticated:
            return Turma.objects.none()
        if user.role == 'ADMIN':
            return Turma.objects.all()
        if user.role == 'TEACHER':
            return Turma.objects.filter(professor=user)
        # Students see classes they are enrolled in
        return Turma.objects.filter(alunos__aluno=user)

    def perform_create(self, serializer):
        serializer.save(professor=self.request.user)

class InscricaoViewSet(viewsets.ModelViewSet):
    serializer_class = InscricaoSerializer
    permission_classes = [permissions.IsAuthenticated]

    def get_queryset(self):
        return Inscricao.objects.filter(aluno=self.request.user)

    def create(self, request, *args, **kwargs):
        serializer = self.get_serializer(data=request.data)
        serializer.is_valid(raise_exception=True)
        
        turma = serializer.context['turma']
        
        if Inscricao.objects.filter(aluno=request.user, turma=turma).exists():
            return Response({"detail": "Já estás inscrito nesta turma."}, status=status.HTTP_400_BAD_REQUEST)
            
        inscricao = Inscricao.objects.create(aluno=request.user, turma=turma)
        return Response(InscricaoSerializer(inscricao).data, status=status.HTTP_201_CREATED)

class SimulacaoViewSet(viewsets.ModelViewSet):
    serializer_class = SimulacaoSerializer
    permission_classes = [permissions.IsAuthenticated]

    def get_queryset(self):
        if self.request.user.role == 'ADMIN':
            return Simulacao.objects.all()
        return Simulacao.objects.filter(user=self.request.user)

    def perform_create(self, serializer):
        serializer.save(user=self.request.user)
