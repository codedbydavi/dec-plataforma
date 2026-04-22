# 💰 DEC – Dinheiro em Casa

## 📌 Descrição

O **DEC – Dinheiro em Casa** é uma plataforma digital de literacia financeira, desenvolvida com o objetivo de simular cenários de economia doméstica e auxiliar os utilizadores na tomada de decisões financeiras informadas.

A aplicação permite a criação de famílias virtuais, registo de rendimentos e despesas, definição de objetivos de poupança e execução de simulações financeiras, promovendo uma aprendizagem prática e interativa.

---

## 🎯 Objetivos

* Promover a literacia financeira em alunos do ensino secundário
* Simular cenários reais de gestão doméstica
* Analisar o impacto de decisões financeiras ao longo do tempo
* Aplicar boas práticas de engenharia de software num projeto full-stack

---

## 🏗️ Arquitetura do Sistema

O projeto segue uma arquitetura distribuída baseada em microserviços:

* **Frontend:** ASP.NET Core MVC
* **Backend:** Django REST API
* **Microserviço:** Python (cálculo financeiro)
* **Base de Dados:** MySQL
* **Reverse Proxy:** Nginx
* **Containerização:** Docker & Docker Compose

---

## 📁 Estrutura do Repositório (Monorepo)

```
/
├── backend/        # API Django
├── frontend/       # Aplicação ASP.NET MVC
├── service/        # Microserviço Python
├── docker-compose.yml
└── .github/
    └── workflows/
        └── ci.yml
```

---

## ⚙️ Tecnologias Utilizadas

* ASP.NET Core
* Django & Django REST Framework
* Python
* MySQL
* Docker & Docker Compose
* Nginx
* Git & GitHub
* GitHub Actions (CI/CD)

---

## 🚀 Como executar o projeto

### 🔧 Pré-requisitos

* Docker
* Docker Compose
* Git

---

### ▶️ Executar com Docker

```bash
git clone <repo-url>
cd DEC

docker-compose up --build
```

A aplicação ficará disponível em:

```
http://localhost
```

---

## 🔄 Integração Contínua (CI)

O projeto utiliza **GitHub Actions** para automatizar:

* Build do frontend (.NET)
* Validação da API Django
* Execução de testes
* Build e execução dos containers Docker

A pipeline está configurada para correr automaticamente em:

* Push para `main` e `dev`
* Pull Requests

---

## 🧪 Testes

Os testes são executados automaticamente na pipeline CI:

* Django → `manage.py test`
* Python → `unittest`
* .NET → `dotnet test`

---

## 📊 Metodologia

O desenvolvimento do projeto segue uma abordagem **Agile (Scrum)**:

* Organização por Sprints
* Gestão de tarefas com GitHub Projects
* User Stories para definição de requisitos
* Sprint Reviews para validação contínua

---

## 👥 Equipa

* **Davi Vasconcelos** – Frontend & Backend
* **Diogo Silva** – Backend, Infraestrutura & DevOps
* **João Maia** – QA, Documentação & Modelação

---

## 📚 Funcionalidades Principais

* Criação de cenários familiares
* Registo de rendimentos e despesas
* Simulação financeira (juros, poupança, crédito)
* Dashboards e análise de dados
* Gestão de turmas e desafios educativos
* Sistema de autenticação de utilizadores

---

## 🔐 Segurança

* Comunicação via HTTPS (Nginx)
* Gestão de autenticação e autorização (Django)
* Isolamento de serviços via Docker

---

## 📈 Estado do Projeto

🚧 Em desenvolvimento (Sprint A)

A arquitetura base, modelação do sistema e pipeline CI encontram-se implementadas. As funcionalidades serão desenvolvidas nas próximas sprints.

---

## 📄 Licença

Este projeto foi desenvolvido para fins académicos no âmbito do curso de Engenharia Informática do ISTEC Porto.

---

## 📌 Notas Finais

O projeto encontra-se em evolução contínua, sendo expectável a adição de novas funcionalidades, testes automatizados e melhorias na arquitetura ao longo do desenvolvimento.

---
