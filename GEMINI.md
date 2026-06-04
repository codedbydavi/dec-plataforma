# GEMINI Context: DEC – Dinheiro em Casa

## Project Overview
**DEC – Dinheiro em Casa** is a digital financial literacy platform designed for secondary school students. It simulates domestic economy scenarios to help users make informed financial decisions.

### Architecture & Stack
- **Frontend:** ASP.NET Core 10.0 MVC (Centralized Security & UI)
- **Backend/Calculation Engine:** Django 6.0 REST API (Financial Calculation Engine)
- **Database:** MySQL 8.0 (Schema v1.0 alingned with ERD)
- **Infrastructure:** Docker, Docker Compose, Nginx as a Reverse Proxy
- **CI/CD:** GitHub Actions

## Quick Access
- **Portal:** `http://localhost`
- **Django Engine API:** `http://localhost/api/calculate/`
- **Django Admin:** `http://localhost/admin/`

## Directory Structure
- `src/frontend/`: ASP.NET Core MVC application (.NET 10.0).
- `src/backend/`: Django project acting as the Financial Calculation Engine.
- `docker-compose.yml`: Root orchestration with database persistence (volumes).
- `nginx/conf.d/default.conf`: Unified gateway routing.

## Development Conventions
- **Monorepo Workflow:** All components under `src/`.
- **Security:** Secrets managed via `.env`. No hardcoded keys.
- **Comments:** Standardized in English.
- **CI/CD:** GitHub Actions validates all components on push/PR.
- **Database:** Managed via Django Migrations. Version control for DDL.

## Building and Running
```bash
# Start the entire ecosystem
docker-compose up --build
```

## Pending/TODO
- [x] Implement financial calculation logic in `src/backend/api/services.py`.
- [ ] Create Student Scenario form in Frontend (US_B015).
- [ ] Implement Simulation History visualization.
