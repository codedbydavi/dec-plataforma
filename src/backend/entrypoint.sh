#!/bin/sh

# exit immediately if a command exits with a non-zero status
set -e

echo "Aguardando o banco de dados iniciar..."

# Lógica simples para esperar o MySQL estar pronto antes de rodar migrações
# Tenta conectar ao DB até ter sucesso
until python manage.py check --database default > /dev/null 2>&1; do
  echo "MySQL ainda não está pronto. A aguardar..."
  sleep 2
done

echo "Banco de dados pronto! A aplicar migrações..."
python manage.py migrate --noinput

echo "Migrações aplicadas. A iniciar o servidor Django..."
exec python manage.py runserver 0.0.0.0:8000
