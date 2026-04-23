from rest_framework import serializers
from .models import Simulacao

class SimulacaoSerializer(serializers.ModelSerializer):
    class Meta:
        model = Simulacao
        fields = '__all__' # Exposes all model fields
