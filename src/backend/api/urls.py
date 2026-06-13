from django.urls import path
from .views import calculate_financials, health_check

urlpatterns = [
    path('calculate/', calculate_financials, name='calculate'),
    path('health/', health_check, name='health'),
]
