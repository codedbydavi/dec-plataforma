# 💰 DEC – Dinheiro em Casa (Money at Home)

<p align="center">
  <em>A digital financial literacy platform designed to simulate household economy scenarios, helping students make informed financial decisions through practical, interactive learning.</em>
</p>

---

## 📌 Overview

**DEC (Dinheiro em Casa)** is an educational platform built for secondary school students. It bridges the gap between theoretical mathematics and real-world financial literacy. By simulating household budgets, evaluating loan impact, projecting compound interest, and adjusting for inflation, the platform empowers students to develop a realistic understanding of personal finance.

The system is structured around two main roles:
*   **Students:** Create family scenarios, record financial entries, set savings goals, run complex mathematical simulations, and submit their work.
*   **Professors:** Create classes, assign pedagogical challenges, monitor student progress, and evaluate submitted simulations with actionable feedback.

Try the demo version at:
<p align="center">
  🔗<em>https://dec-plataforma.duckdns.org/</em>
</p>
---

## ✨ Key Features

### 🎓 For Students
*   **Virtual Family Scenarios:** Define starting balances and aggregate incomes/expenses to build realistic monthly budgets.
*   **Financial Engine (What-If Analysis):**
    *   **Base Simulation:** Projects the monthly balance based on recurring entries.
    *   **Credit Simulation:** Calculates monthly installments, total interest, and effort rate (using the French amortization system).
    *   **Savings Projection:** Computes compound interest over a specified term.
    *   **Cash Flow & Inflation:** Projects the impact of inflation on variable expenses over time.
*   **Challenge Submissions:** Respond to professor-assigned challenges directly using customized scenarios.

### 👨‍🏫 For Professors
*   **Classroom Management:** Generate unique Join Codes for students to enroll seamlessly.
*   **Pedagogical Challenges:** Create and assign tailored financial tasks with integrated external resources.
*   **Real-time Dashboard:** Monitor student performance, including average scores, completion rates, and pending evaluations.
*   **Evaluation System:** Review student simulations, assess financial sustainability (e.g., effort rates), and provide targeted feedback.

---

## 🏗️ System Architecture

The project follows a distributed, highly decoupled architecture:

*   **Frontend (UI & Security):** Built with **ASP.NET Core 10.0 MVC**, utilizing TailwindCSS for a responsive, modern interface. It centralizes Identity Management (Auth), securing routes via JWT and HttpOnly Cookies.
*   **Backend (Financial Engine):** Developed in **Django 6.0 (Python 3.12)**. It acts as the mathematical core, processing heavy financial simulations via a RESTful API.
*   **Database:** **MySQL 8.0**, fully modeled to support relational integrity between classrooms, scenarios, entries, and simulation histories.
*   **Infrastructure:** Containerized with **Docker**, orchestrated via **Docker Compose**, and routed through an **Nginx** reverse proxy acting as an API Gateway.

---

## 🚀 Deployment & CI/CD

The platform is configured for production readiness, leveraging cloud infrastructure and automated pipelines:

*   **Hosting:** Deployed on **Google Cloud Platform (GCP)** Compute Engine.
*   **SSL/Security:** Fully secured with HTTPS using **Let's Encrypt / Certbot**.
*   **CI/CD Pipeline:** Powered by **GitHub Actions**.
    *   **Continuous Integration:** Automatically runs builds and sanity checks (Python & .NET) on every push.
    *   **Continuous Deployment:** Builds Docker images as Artifacts, pushes them to the **GitHub Container Registry (GHCR)**, and triggers a rolling update on the GCP server via SSH.

---

## 💻 How to Run Locally

To run the application in a local development environment:

### Prerequisites
*   [Docker Desktop](https://www.docker.com/products/docker-desktop) installed and running.
*   Git installed.

### Quick Start

1. Clone the repository:
   ```bash
   git clone https://github.com/codedbydavi/dec-plataforma.git
   cd dec-plataforma
   ```

2. Start the development environment:
   ```bash
   # Uses the dev-specific compose file without SSL requirements
   docker-compose -f docker-compose.dev.yml up -d --build
   ```

3. Access the platform:
   *   **Frontend Application:** [http://localhost](http://localhost)
   *   **Django Engine API:** [http://localhost:8000/api/](http://localhost:8000/api/)
   *   **Django Admin Panel:** [http://localhost:8000/admin/](http://localhost:8000/admin/)

---

## 👥 Development Team

*   **Davi Vasconcelos** – Frontend Architecture, Cloud Infrastructure (GCP) & CI/CD
*   **Diogo Silva** – Backend Development (Django Engine) & DevOps
*   **João Maia** – QA, Database Modeling & Documentation

---

## 📄 License

Developed for academic purposes as the final project for the Computer Science degree at ISTEC Porto. All rights reserved by the authors.
