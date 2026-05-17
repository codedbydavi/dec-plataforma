from django.urls import path, include
from rest_framework.routers import DefaultRouter
from rest_framework_simplejwt.views import TokenRefreshView
from .views import (
    CustomTokenObtainPairView,
    ClassGroupViewSet,
    EnrollmentViewSet,
    ScenarioViewSet,
    EntryViewSet,
    ObjectiveViewSet,
    SimulationHistoryViewSet
)

router = DefaultRouter()
router.register(r'classes', ClassGroupViewSet, basename='class')
router.register(r'enrollments', EnrollmentViewSet, basename='enrollment')
router.register(r'scenarios', ScenarioViewSet, basename='scenario')
router.register(r'entries', EntryViewSet, basename='entry')
router.register(r'objectives', ObjectiveViewSet, basename='objective')
router.register(r'histories', SimulationHistoryViewSet, basename='history')

urlpatterns = [
    path('', include(router.urls)),
    path('token/', CustomTokenObtainPairView.as_view(), name='token_obtain_pair'),
    path('token/refresh/', TokenRefreshView.as_view(), name='token_refresh'),
]
