from rest_framework import viewsets, permissions, status
from rest_framework.response import Response
from rest_framework_simplejwt.views import TokenObtainPairView
from .models import (
    CustomUser, Turma, Inscricao, Scenario, Entry, Objective, SimulationHistory
)
from .serializers import (
    CustomTokenObtainPairSerializer, 
    UserSerializer, 
    TurmaSerializer, 
    InscricaoSerializer,
    ScenarioSerializer,
    EntrySerializer,
    ObjectiveSerializer,
    SimulationHistorySerializer
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

# --- Novas Views alinhadas com o Diagrama ER ---

class ScenarioViewSet(viewsets.ModelViewSet):
    serializer_class = ScenarioSerializer
    permission_classes = [permissions.IsAuthenticated]

    def get_queryset(self):
        if self.request.user.role == 'ADMIN':
            return Scenario.objects.all()
        return Scenario.objects.filter(student=self.request.user)

    def perform_create(self, serializer):
        serializer.save(student=self.request.user)

class EntryViewSet(viewsets.ModelViewSet):
    serializer_class = EntrySerializer
    permission_classes = [permissions.IsAuthenticated]

    def get_queryset(self):
        return Entry.objects.filter(scenario__student=self.request.user)

class ObjectiveViewSet(viewsets.ModelViewSet):
    serializer_class = ObjectiveSerializer
    permission_classes = [permissions.IsAuthenticated]

    def get_queryset(self):
        return Objective.objects.filter(scenario__student=self.request.user)

class SimulationHistoryViewSet(viewsets.ModelViewSet):
    serializer_class = SimulationHistorySerializer
    permission_classes = [permissions.IsAuthenticated]

    def get_queryset(self):
        return SimulationHistory.objects.filter(scenario__student=self.request.user)
