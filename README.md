# 💰 DEC – Dinheiro em Casa (Money at Home)

## 📌 Description

**DEC – Dinheiro em Casa** is a digital financial literacy platform developed to simulate household economy scenarios and assist students in making informed financial decisions.

The application allows for the creation of virtual family scenarios, recording income and expenses, defining savings goals, and performing financial simulations, promoting practical and interactive learning.

---

## 🚀 Quick Access (Local Environment)

If the system is running via Docker, use these links for quick access:

*   **🌐 Frontend Portal:** [http://localhost](http://localhost)
*   **📖 API Documentation (Swagger):** [http://localhost/api/docs/](http://localhost/api/docs/)
*   **🛡️ Admin Panel (Django):** [http://localhost/admin/](http://localhost/admin/)
*   **📊 API Redoc:** [http://localhost/api/redoc/](http://localhost/api/redoc/)

---

## ✅ Implementation Progress (User Stories)

The following User Stories have been successfully implemented:

| ID | Role | Requirement |
|:---|:---|:---|
| **US_B004** | Developer | Implementation of Authentication (JWT) and Session/Token management. |
| **US_B005** | Manager | Implementation of Roles and Permissions (Admin, Teacher, Student). |
| **US_B006** | Developer | Centralized security in .NET MVC Portal communicating securely with Django API. |
| **US_B007** | DB Manager | Definition of MySQL schema (tables, keys, and optimized indexes) v1.0. |
| **US_B008** | Developer | Automatic Database Migrations and DDL version control. |
| **US_B009** | Developer | Python Microservice skeleton with Clean Architecture and Health-check. |
| **US_B010** | DevOps | Full Docker Compose orchestration (Portal + API + Service + MySQL). |
| **US_B011** | DevOps | Structured logging implementation in API and Microservice. |
| **US_B012** | Admin | Class/Challenge creation with unique Join Codes (DEC-XXXXXX). |
| **US_B013** | Student | Joining a class/challenge using a Join Code. |
| **US_B014** | Teacher | Dashboard to list classes, join codes, and enrolled students. |

---

## 🏗️ System Architecture

The project follows a distributed architecture based on **Clean Architecture** and **Microservices**:

*   **Frontend:** ASP.NET Core 10.0 MVC (Centralized Security & UI)
*   **Backend:** Django 6.0 REST API (Business Logic & Data Persistence)
*   **Microservice:** Python Flask (Financial Calculation Engine)
*   **Database:** MySQL 8.0 (Relational Data Model v1.0)
*   **Reverse Proxy:** Nginx (Gateway & Unified Routing)
*   **Containerization:** Docker & Docker Compose

---

## ⚙️ Technologies & Security

*   **Auth:** JWT (JSON Web Tokens) + Secure HttpOnly Cookies.
*   **Environment:** All secrets (SECRET_KEY, DB credentials) managed via `.env`.
*   **Persistence:** Docker Volumes for MySQL data.
*   **Documentation:** Automatic OpenAPI 3.0 generation with Swagger UI.
*   **Architecture:** Dependency Injection, Service Layer, and Repository Pattern principles.

---

## 🚀 How to Run the Project

### 🔧 Prerequisites

*   Docker & Docker Compose
*   Git

### ▶️ Run with Docker (Recommended)

```bash
git clone <repo-url>
cd dec-plataforma

# The system will automatically build images, wait for MySQL, and apply migrations
docker-compose up --build
```

---

## 📁 Repository Structure (Monorepo)

```
/
├── src/
│   ├── backend/    # Django API (Python 3.14)
│   ├── frontend/   # ASP.NET MVC Application (.NET 10.0)
│   └── service/    # Financial Microservice (Python 3.14)
├── nginx/          # Nginx Gateway Configuration
├── docker-compose.yml
└── dec-plataforma.sln
```

---

## 📊 Methodology

The project development follows an **Agile (Scrum)** approach:

*   Organized by Sprints
*   Task management with GitHub Projects
*   User Stories for requirements definition
*   Sprint Reviews for continuous validation

---

## 👥 Team

*   **Davi Vasconcelos** – Frontend & Backend
*   **Diogo Silva** – Backend, Infrastructure & DevOps
*   **João Maia** – QA, Documentation & Modeling

---

## 📄 License

This project was developed for academic purposes as part of the Computer Science course at ISTEC Porto.
