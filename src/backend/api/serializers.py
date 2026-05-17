from rest_framework import serializers
from rest_framework_simplejwt.serializers import TokenObtainPairSerializer
from .models import CustomUser, Simulacao

class CustomTokenObtainPairSerializer(TokenObtainPairSerializer):
    @classmethod
    def get_token(cls, user):
        token = super().get_token(user)
        # Add custom claims
        token['role'] = user.role
        token['username'] = user.username
        return token

    def validate(self, attrs):
        data = super().validate(attrs)
        data['role'] = self.user.role
        data['username'] = self.user.username
        return data

class TurmaSerializer(serializers.ModelSerializer):
    professor_name = serializers.ReadOnlyField(source='professor.username')
    aluno_count = serializers.SerializerMethodField()

    class Meta:
        model = Turma
        fields = ('id', 'nome', 'professor', 'professor_name', 'codigo_adesao', 'aluno_count', 'data_criacao')
        read_only_fields = ('professor', 'codigo_adesao')

    def get_aluno_count(self, obj):
        return obj.alunos.count()

class InscricaoSerializer(serializers.ModelSerializer):
    turma_nome = serializers.ReadOnlyField(source='turma.nome')
    codigo_adesao = serializers.CharField(write_only=True)

    class Meta:
        model = Inscricao
        fields = ('id', 'turma', 'turma_nome', 'codigo_adesao', 'data_adesao')
        read_only_fields = ('turma', 'data_adesao')

    def validate_codigo_adesao(self, value):
        try:
            turma = Turma.objects.get(codigo_adesao=value)
            self.context['turma'] = turma
        except Turma.DoesNotExist:
            raise serializers.ValidationError("Código de adesão inválido.")
        return value

class UserSerializer(serializers.ModelSerializer):
...    class Meta:
        model = CustomUser
        fields = ('id', 'username', 'email', 'role')

class SimulacaoSerializer(serializers.ModelSerializer):
    class Meta:
        model = Simulacao
        fields = '__all__'
        read_only_fields = ('user',)
