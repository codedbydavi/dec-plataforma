from django.urls import path, include
from rest_framework.routers import DefaultRouter
from .views import SimulacaoViewSet

# Router automatically creates routes for CRUD (GET, POST, etc.)
router = DefaultRouter()
router.register(r'simulacoes', SimulacaoViewSet)

urlpatterns = [
    path('', include(router.urls)),
]
