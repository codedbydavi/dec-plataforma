#!/bin/sh

# exit immediately if a command exits with a non-zero status
set -e

echo "Waiting for database to start..."

# Simple logic to wait for MySQL to be ready before running migrations
# Try connecting to the DB until successful
until python manage.py check --database default > /dev/null 2>&1; do
  echo "MySQL is not ready yet. Waiting..."
  sleep 2
done

echo "Database ready! Applying migrations..."
python manage.py migrate --noinput

echo "Migrations applied. Starting Django server..."
exec python manage.py runserver 0.0.0.0:8000
