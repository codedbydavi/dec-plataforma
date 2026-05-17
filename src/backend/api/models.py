from django.db import models
from django.contrib.auth.models import AbstractUser
import uuid

# --- Tabelas de Suporte (Lookups) ---

class Role(models.Model):
    role = models.CharField(max_length=50, unique=True)
    def __str__(self): return self.role

class UserStatus(models.Model):
    status = models.CharField(max_length=50, unique=True)
    def __str__(self): return self.status

class ClassStatus(models.Model):
    status = models.CharField(max_length=50, unique=True)
    def __str__(self): return self.status

class Gender(models.Model):
    name = models.CharField(max_length=50, unique=True)
    def __str__(self): return self.name

# --- Utilizadores ---

class CustomUser(AbstractUser):
    name = models.CharField(max_length=255, blank=True)
    gender = models.ForeignKey(Gender, on_delete=models.SET_NULL, null=True, blank=True)
    birth_date = models.DateField(null=True, blank=True)
    img_url = models.URLField(max_length=500, null=True, blank=True)
    user_status = models.ForeignKey(UserStatus, on_delete=models.SET_NULL, null=True, blank=True)
    role_entity = models.ForeignKey(Role, on_delete=models.SET_NULL, null=True, blank=True)
    
    # Mantemos o campo role original para compatibilidade com o código atual de permissões
    ROLE_CHOICES = (
        ('ADMIN', 'Admin'),
        ('TEACHER', 'Professor'),
        ('STUDENT', 'Aluno'),
    )
    role = models.CharField(max_length=10, choices=ROLE_CHOICES, default='STUDENT', db_index=True)

    def __str__(self):
        return f"{self.username} ({self.role})"

# --- Turmas ---

class Turma(models.Model):
    nome = models.CharField(max_length=100)
    professor = models.ForeignKey(CustomUser, on_delete=models.CASCADE, related_name='turmas_criadas')
    codigo_adesao = models.CharField(max_length=10, unique=True, db_index=True)
    class_status = models.ForeignKey(ClassStatus, on_delete=models.SET_NULL, null=True, blank=True)
    data_criacao = models.DateTimeField(auto_now_add=True)

    def save(self, *args, **kwargs):
        if not self.codigo_adesao:
            self.codigo_adesao = f"DEC-{uuid.uuid4().hex[:6].upper()}"
        super().save(*args, **kwargs)

    def __str__(self):
        return f"{self.nome} ({self.codigo_adesao})"

class Inscricao(models.Model):
    aluno = models.ForeignKey(CustomUser, on_delete=models.CASCADE, related_name='inscricoes')
    turma = models.ForeignKey(Turma, on_delete=models.CASCADE, related_name='alunos')
    data_adesao = models.DateTimeField(auto_now_add=True)

    class Meta:
        unique_together = ('aluno', 'turma')
        verbose_name = 'Inscrição'
        verbose_name_plural = 'Inscrições'

# --- Cenários e Simulações (O Coração do Projeto) ---

class Scenario(models.Model):
    student = models.ForeignKey(CustomUser, on_delete=models.CASCADE, related_name='scenarios')
    family_name = models.CharField(max_length=100)
    initial_balance = models.DecimalField(max_digits=12, decimal_places=2, default=0.0)
    data_criacao = models.DateTimeField(auto_now_add=True, db_index=True)

    def __str__(self):
        return f"{self.family_name} - {self.student.username}"

class Entry(models.Model):
    ENTRY_TYPES = (('INCOME', 'Receita'), ('EXPENSE', 'Despesa'))
    
    scenario = models.ForeignKey(Scenario, on_delete=models.CASCADE, related_name='entries')
    type = models.CharField(max_length=10, choices=ENTRY_TYPES)
    category = models.CharField(max_length=100)
    amount = models.DecimalField(max_digits=12, decimal_places=2)
    month = models.IntegerField()  # 1-12
    recurrence = models.BooleanField(default=False)

    def __str__(self):
        return f"{self.type}: {self.category} ({self.amount})"

class Objective(models.Model):
    scenario = models.ForeignKey(Scenario, on_delete=models.CASCADE, related_name='objectives')
    description = models.CharField(max_length=255)
    target_value = models.DecimalField(max_digits=12, decimal_places=2)
    term_months = models.IntegerField()

    def __str__(self):
        return self.description

class SimulationHistory(models.Model):
    scenario = models.ForeignKey(Scenario, on_delete=models.CASCADE, related_name='histories')
    execution_date = models.DateTimeField(auto_now_add=True)
    json_results = models.JSONField()

    def __str__(self):
        return f"Sim: {self.scenario.family_name} @ {self.execution_date}"
