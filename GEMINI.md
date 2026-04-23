# GEMINI Context: DEC – Dinheiro em Casa

## Project Overview
**DEC – Dinheiro em Casa** is a digital financial literacy platform designed for secondary school students. It simulates domestic economy scenarios to help users make informed financial decisions. The project uses a microservices-based monorepo architecture.

### Architecture & Stack
- **Frontend:** ASP.NET Core MVC (located in `src/frontend`)
- **Backend:** Django REST API (located in `src/backend`)
- **Microservice:** Python for financial calculations (located in `src/service`)
- **Database:** MySQL
- **Infrastructure:** Docker, Docker Compose, Nginx as a Reverse Proxy
- **CI/CD:** GitHub Actions

## Directory Structure
- `src/frontend/`: ASP.NET Core MVC application (.NET 10.0 based on `.csproj`, though CI uses 7.0).
- `src/backend/`: Django project using Django REST Framework and CORS headers.
- `src/service/`: Python microservice skeleton.
- `docker-compose.yml`: Root orchestration for all services, including a MySQL database and Nginx.
- `.github/workflows/ci.yml`: Automated pipeline for building and testing all components.

## Building and Running

### Full Stack (Docker)
The entire system can be started using Docker Compose from the root directory:
```bash
docker-compose up --build
```
The application becomes available at `http://localhost` via Nginx.

### Backend (Django)
1. Navigate to `src/backend`.
2. Activate virtual environment: `source venv/bin/activate`.
3. Install dependencies: `pip install -r requirements.txt`.
4. Run server: `python manage.py runserver`.
5. Run tests: `python manage.py test`.

### Frontend (.NET)
1. Navigate to `src/frontend`.
2. Restore dependencies: `dotnet restore`.
3. Build project: `dotnet build`.
4. Run project: `dotnet run`.
5. Run tests: `dotnet test`.

### Microservice (Python)
1. Navigate to `src/service`.
2. Run microservice: `python main.py`.
3. Run tests: `python -m unittest discover`.

## Development Conventions
- **Monorepo Workflow:** All components are housed under the `src/` directory.
- **CI/CD:** GitHub Actions validates Django, .NET, and Docker builds on every push to `main` and `dev`.
- **Dockerization:** Each service has its own `Dockerfile` for consistent deployment.
- **RESTful API:** The backend is designed as a REST API to serve the frontend and potentially other clients.

## Pending/TODO
- [ ] Align .NET versions between `Frontend.csproj` (10.0) and `ci.yml` (7.0).
- [ ] Implement financial calculation logic in `src/service/main.py`.
- [ ] Configure Nginx rules for routing between Frontend and Backend in a dedicated config file (referenced in `docker-compose.yml` but directory not yet present).
