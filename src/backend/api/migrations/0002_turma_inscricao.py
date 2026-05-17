import django.db.models.deletion
from django.conf import settings
from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('api', '0001_initial'),
    ]

    operations = [
        migrations.CreateModel(
            name='Turma',
            fields=[
                ('id', models.BigAutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('nome', models.CharField(max_length=100)),
                ('codigo_adesao', models.CharField(db_index=True, max_length=10, unique=True)),
                ('data_criacao', models.DateTimeField(auto_now_add=True)),
                ('professor', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, related_name='turmas_criadas', to=settings.AUTH_USER_MODEL)),
            ],
        ),
        migrations.CreateModel(
            name='Inscricao',
            fields=[
                ('id', models.BigAutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('data_adesao', models.DateTimeField(auto_now_add=True)),
                ('aluno', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, related_name='inscricoes', to=settings.AUTH_USER_MODEL)),
                ('turma', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, related_name='alunos', to='api.turma')),
            ],
            options={
                'verbose_name': 'Inscrição',
                'verbose_name_plural': 'Inscrições',
                'unique_together': {('aluno', 'turma')},
            },
        ),
    ]
