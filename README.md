# 💰 DEC – Dinheiro em Casa (Money at Home)

## 📌 Description

**DEC – Dinheiro em Casa** is a digital financial literacy platform, developed with the goal of simulating domestic economy scenarios and assisting users in making informed financial decisions.

The application allows for the creation of virtual families, recording income and expenses, defining savings goals, and performing financial simulations, promoting practical and interactive learning.

---

## 🎯 Objectives

* Promote financial literacy among secondary school students
* Simulate real domestic management scenarios
* Analyze the impact of financial decisions over time
* Apply software engineering best practices in a full-stack project

---

## 🏗️ System Architecture

The project follows a distributed architecture based on microservices:

* **Frontend:** ASP.NET Core MVC
* **Backend:** Django REST API
* **Microservice:** Python (financial calculation)
* **Database:** MySQL
* **Reverse Proxy:** Nginx
* **Containerization:** Docker & Docker Compose

---

## 📁 Repository Structure (Monorepo)

```
/
├── src/
│   ├── backend/    # Django API
│   ├── frontend/   # ASP.NET MVC Application
│   └── service/    # Python Microservice
├── docker-compose.yml
└── .github/
    └── workflows/
        └── ci.yml
```

---

## ⚙️ Technologies Used

* ASP.NET Core
* Django & Django REST Framework
* Python
* MySQL
* Docker & Docker Compose
* Nginx
* Git & GitHub
* GitHub Actions (CI/CD)

---

## 🚀 How to Run the Project

### 🔧 Prerequisites

* Docker
* Docker Compose
* Git

---

### ▶️ Run with Docker

```bash
git clone <repo-url>
cd DEC

docker-compose up --build
```

The application will be available at:

```
http://localhost
```

---

## 🔄 Continuous Integration (CI)

The project uses **GitHub Actions** to automate:

* Frontend build (.NET)
* Django API validation
* Test execution
* Docker container build and execution

The pipeline is configured to run automatically on:

* Push to `main` and `dev`
* Pull Requests

---

## 🧪 Testing

Tests are executed automatically in the CI pipeline:

* Django → `manage.py test`
* Python → `unittest`
* .NET → `dotnet test`

---

## 📊 Methodology

The project development follows an **Agile (Scrum)** approach:

* Organized by Sprints
* Task management with GitHub Projects
* User Stories for requirements definition
* Sprint Reviews for continuous validation

---

## 👥 Team

* **Davi Vasconcelos** – Frontend & Backend
* **Diogo Silva** – Backend, Infrastructure & DevOps
* **João Maia** – QA, Documentation & Modeling

---

## 📚 Key Features

* Creation of family scenarios
* Recording of income and expenses
* Financial simulation (interest, savings, credit)
* Dashboards and data analysis
* Management of classes and educational challenges
* User authentication system

---

## 🔐 Security

* Communication via HTTPS (Nginx)
* Authentication and authorization management (Django)
* Service isolation via Docker

---

## 📈 Project Status

🚧 Under development (Sprint A)

The base architecture, system modeling, and CI pipeline are implemented. Features will be developed in the upcoming sprints.

---

## 📄 License

This project was developed for academic purposes as part of the Computer Engineering course at ISTEC Porto.

---

## 📌 Final Notes

The project is in continuous evolution, with new features, automated tests, and architectural improvements expected throughout development.
