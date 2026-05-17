from rest_framework import viewsets, permissions, status
from rest_framework.response import Response
from rest_framework_simplejwt.views import TokenObtainPairView
from .models import (
    CustomUser, ClassGroup, Enrollment, Scenario, Entry, Objective, SimulationHistory
)
from .serializers import (
    CustomTokenObtainPairSerializer, 
    UserSerializer, 
    ClassGroupSerializer, 
    EnrollmentSerializer,
    ScenarioSerializer,
    EntrySerializer,
    ObjectiveSerializer,
    SimulationHistorySerializer
)
from .permissions import IsAdmin, IsTeacher, IsStudent

class CustomTokenObtainPairView(TokenObtainPairView):
    serializer_class = CustomTokenObtainPairSerializer

class ClassGroupViewSet(viewsets.ModelViewSet):
    serializer_class = ClassGroupSerializer
    
    def get_permissions(self):
        if self.action in ['create', 'update', 'partial_update', 'destroy']:
            return [(IsTeacher | IsAdmin)()]
        return [permissions.IsAuthenticated()]

    def get_queryset(self):
        user = self.request.user
        if not user.is_authenticated:
            return ClassGroup.objects.none()
        if user.role == 'ADMIN':
            return ClassGroup.objects.all()
        if user.role == 'TEACHER':
            return ClassGroup.objects.filter(teacher=user)
        return ClassGroup.objects.filter(students__student=user)

    def perform_create(self, serializer):
        serializer.save(teacher=self.request.user)

class EnrollmentViewSet(viewsets.ModelViewSet):
    serializer_class = EnrollmentSerializer
    permission_classes = [permissions.IsAuthenticated]

    def get_queryset(self):
        return Enrollment.objects.filter(student=self.request.user)

    def create(self, request, *args, **kwargs):
        serializer = self.get_serializer(data=request.data)
        serializer.is_valid(raise_exception=True)
        class_group = serializer.context['class_group']
        if Enrollment.objects.filter(student=request.user, class_group=class_group).exists():
            return Response({"detail": "You are already enrolled in this class."}, status=status.HTTP_400_BAD_REQUEST)
        enrollment = Enrollment.objects.create(student=request.user, class_group=class_group)
        return Response(EnrollmentSerializer(enrollment).data, status=status.HTTP_201_CREATED)

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
