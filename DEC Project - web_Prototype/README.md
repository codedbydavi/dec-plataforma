# DEC - Dinheiro em Casa

Plataforma de Literacia Financeira para Alunos do Ensino Secundário (10º ao 12º ano) desenvolvida para o ISTEC.

## Descrição

DEC (Dinheiro em Casa) é uma aplicação web que simula cenários reais de gestão doméstica através de "famílias virtuais". A plataforma permite aos alunos criar e gerir orçamentos familiares, registar transações, realizar simulações financeiras e completar desafios pedagógicos.

## Tecnologias

- **Framework**: ASP.NET Core MVC 8.0
- **Linguagem**: C# 12
- **Base de Dados**: MySQL (preparado para integração futura)
- **Frontend**: Razor Views, CSS3, JavaScript
- **Arquitetura**: MVC Pattern com Mock Data Service

## Funcionalidades

### Para Alunos
- ✅ Criação e gestão de famílias virtuais
- ✅ Registo de rendimentos e despesas (fixas e variáveis)
- ✅ Cálculo automático da taxa de esforço
- ✅ Simulações financeiras:
  - Juros compostos com contribuições mensais
  - Amortização de crédito habitação
  - Projeção com inflação
- ✅ Sistema de desafios pedagógicos
- ✅ Dashboard personalizado com estatísticas

### Para Professores
- ✅ Criação e gestão de desafios
- ✅ Visualização de alunos registados
- ✅ Dashboard com estatísticas da turma
- ✅ Acompanhamento de famílias criadas

### Para Administradores
- ✅ Gestão completa de utilizadores
- ✅ Monitorização de toda a plataforma
- ✅ Estatísticas globais do sistema
- ✅ Gestão de desafios

## Estrutura do Projeto

```
DEC/
├── Controllers/          # Controladores MVC
│   ├── HomeController.cs
│   ├── DashboardController.cs
│   ├── FamiliaController.cs
│   ├── SimulacaoController.cs
│   └── DesafioController.cs
├── Models/              # Modelos de dados
│   ├── User.cs
│   ├── Familia.cs
│   ├── Transacao.cs
│   ├── Simulacao.cs
│   └── Desafio.cs
├── Views/               # Views Razor
│   ├── Home/
│   ├── Dashboard/
│   ├── Familia/
│   ├── Simulacao/
│   ├── Desafio/
│   └── Shared/
├── Data/                # Contexto e acesso a dados
│   └── ApplicationDbContext.cs
├── Services/            # Serviços
│   └── MockDataService.cs
├── wwwroot/            # Recursos estáticos
│   ├── css/
│   └── images/
├── Program.cs          # Configuração da aplicação
└── DEC.csproj         # Arquivo de projeto
```

## Como Executar

### Pré-requisitos
- .NET 8.0 SDK instalado
- Visual Studio 2022 ou VS Code
- MySQL (opcional - para versão final com base de dados)

### Passos

1. **Clone ou navegue até o diretório do projeto**
```bash
cd /workspaces/default/code
```

2. **Restaure os pacotes NuGet**
```bash
dotnet restore
```

3. **Execute a aplicação**
```bash
dotnet run
```

4. **Acesse no navegador**
```
https://localhost:5001
ou
http://localhost:5000
```

## Contas de Teste

### Aluno
- Email: `joao@istec.pt`
- Password: `aluno123`

### Professor
- Email: `prof.silva@istec.pt`
- Password: `prof123`

### Administrador
- Email: `admin@istec.pt`
- Password: `admin123`

## Diretrizes Visuais ISTEC

A plataforma segue as diretrizes visuais do ISTEC:

### Cores
- **Azul Principal**: #0C73B7
- **Azul Claro**: #2da7df
- **Preto**: #1d1d1b
- **Branco**: #FFFFFF

### Tipografia
- **Títulos**: Roboto (400, 500, 700)
- **Texto**: Montserrat (300, 400, 500, 600)

### Logos
Os logos do ISTEC estão localizados em `/wwwroot/images/`.

## Próximos Passos

### Integração com MySQL
1. Descomente a configuração do DbContext em `Program.cs`
2. Configure a connection string em `appsettings.json`
3. Execute as migrations:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Autenticação Real
- Implementar hash de passwords (BCrypt ou ASP.NET Identity)
- Adicionar autenticação por cookies
- Implementar recuperação de password

### Funcionalidades Adicionais
- Sistema de badges e recompensas
- Relatórios e exportação de dados
- Gráficos interativos com Chart.js
- Sistema de notificações
- Ranking de alunos

## Arquitetura Futura

O projeto está preparado para evoluir para uma arquitetura distribuída:

1. **Frontend**: React/Angular (separado)
2. **Backend MVC**: ASP.NET Core MVC (interface web)
3. **API Django**: Python/Django REST Framework (lógica de negócio)
4. **Microserviço Python**: Cálculos financeiros avançados
5. **Base de Dados**: MySQL

## Estrutura de Dados

### Principais Entidades
- **User**: Utilizadores do sistema (Aluno, Professor, Administrador)
- **Familia**: Cenários familiares criados pelos alunos
- **Transacao**: Rendimentos e despesas das famílias
- **Simulacao**: Simulações financeiras realizadas
- **Desafio**: Desafios pedagógicos criados pelos professores
- **DesafioAluno**: Relação entre desafios e alunos
- **Badge**: Badges de conquista

## Contribuição

Este é um projeto académico desenvolvido para o ISTEC. Para sugestões ou melhorias, contacte a equipa de desenvolvimento.

## Licença

© 2026 ISTEC - Instituto Superior de Tecnologias Avançadas. Todos os direitos reservados.

## Suporte

Para questões técnicas ou pedagógicas, contacte:
- Email: suporte.dec@istec.pt
- Website: https://istec.pt

---

**Desenvolvido com ❤️ para promover a literacia financeira nas escolas portuguesas**
