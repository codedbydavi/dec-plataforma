from django.db import models
from django.contrib.auth.models import AbstractUser

class CustomUser(AbstractUser):
    ROLE_CHOICES = (
        ('ADMIN', 'Admin'),
        ('TEACHER', 'Professor'),
        ('STUDENT', 'Aluno'),
    )
    role = models.CharField(max_length=10, choices=ROLE_CHOICES, default='STUDENT', db_index=True)

    def __str__(self):
        return f"{self.username} ({self.role})"

import uuid

class Turma(models.Model):
    nome = models.CharField(max_length=100)
    professor = models.ForeignKey(CustomUser, on_delete=models.CASCADE, related_name='turmas_criadas')
    codigo_adesao = models.CharField(max_length=10, unique=True, db_index=True)
    data_criacao = models.DateTimeField(auto_now_add=True)

    def save(self, *args, **kwargs):
        if not self.codigo_adesao:
            # Gerar código único e curto (ex: DEC-XXXX)
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

class Simulacao(models.Model):
...    user = models.ForeignKey(CustomUser, on_delete=models.CASCADE, related_name='simulacoes', null=True, db_index=True)
    titulo = models.CharField(max_length=100)
    rendimento_mensal = models.DecimalField(max_digits=10, decimal_places=2)
    despesas_mensais = models.DecimalField(max_digits=10, decimal_places=2)
    poupanca_estimada = models.DecimalField(max_digits=10, decimal_places=2, null=True, blank=True)
    data_criacao = models.DateTimeField(auto_now_add=True, db_index=True)

    class Meta:
        indexes = [
            models.Index(fields=['user', 'data_criacao']),
        ]
        verbose_name = 'Simulação'
        verbose_name_plural = 'Simulações'

    def __str__(self):
        return self.titulo
