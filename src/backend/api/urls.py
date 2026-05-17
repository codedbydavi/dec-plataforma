from django.urls import path, include
from rest_framework.routers import DefaultRouter
from rest_framework_simplejwt.views import TokenRefreshView
from .views import SimulacaoViewSet, CustomTokenObtainPairView

router = DefaultRouter()
router.register(r'simulacoes', SimulacaoViewSet, basename='simulacao')
router.register(r'turmas', TurmaViewSet, basename='turma')
router.register(r'inscricoes', InscricaoViewSet, basename='inscricao')

urlpatterns = [
    path('', include(router.urls)),
    path('token/', CustomTokenObtainPairView.as_view(), name='token_obtain_pair'),
    path('token/refresh/', TokenRefreshView.as_view(), name='token_refresh'),
]
