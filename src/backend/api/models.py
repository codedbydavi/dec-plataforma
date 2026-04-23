from django.db import models

class Simulacao(models.Model):
    titulo = models.CharField(max_length=100)
    rendimento_mensal = models.DecimalField(max_digits=10, decimal_places=2)
    despesas_mensais = models.DecimalField(max_digits=10, decimal_places=2)
    poupanca_estimada = models.DecimalField(max_digits=10, decimal_places=2, null=True, blank=True)
    data_criacao = models.DateTimeField(auto_now_add=True)

    def __str__(self):
        return self.titulo
