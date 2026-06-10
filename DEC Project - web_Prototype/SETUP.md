# Guia de Configuração - DEC (Dinheiro em Casa)

## Início Rápido

### 1. Verificar Pré-requisitos

```bash
# Verificar se o .NET SDK está instalado
dotnet --version
# Deve mostrar versão 8.0 ou superior
```

Se não tiver o .NET instalado:
- **Windows**: Baixar de https://dotnet.microsoft.com/download
- **macOS**: `brew install dotnet`
- **Linux**: `sudo apt-get install dotnet-sdk-8.0`

### 2. Restaurar Dependências

```bash
cd /workspaces/default/code
dotnet restore
```

### 3. Executar a Aplicação

```bash
dotnet run
```

A aplicação estará disponível em:
- HTTPS: `https://localhost:5001`
- HTTP: `http://localhost:5000`

### 4. Primeiro Acesso

Use uma das contas de teste:

**Aluno:**
- Email: `joao@istec.pt`
- Password: `aluno123`

**Professor:**
- Email: `prof.silva@istec.pt`
- Password: `prof123`

**Administrador:**
- Email: `admin@istec.pt`
- Password: `admin123`

## Estrutura do Projeto

```
DEC/
├── Controllers/          → Lógica de controlo MVC
├── Models/              → Entidades de dados
├── Views/               → Interface visual (Razor)
├── Data/                → Contexto de base de dados
├── Services/            → Serviços de negócio
└── wwwroot/            → Arquivos estáticos (CSS, JS, imagens)
```

## Funcionalidades Principais

### Para Alunos
1. **Criar Família**
   - Aceder a "Minhas Famílias" → "Nova Família"
   - Preencher informações básicas
   - Adicionar rendimentos e despesas

2. **Gerir Transações**
   - Na página de detalhes da família
   - Clicar em "Nova Transação"
   - A taxa de esforço é calculada automaticamente

3. **Realizar Simulações**
   - Aceder a "Simulações" na família
   - Escolher tipo de simulação:
     - Juros Compostos
     - Amortização de Crédito
     - Projeção com Inflação

4. **Completar Desafios**
   - Aceder a "Desafios"
   - Ver detalhes do desafio
   - Completar as tarefas propostas

### Para Professores
1. **Criar Desafios**
   - Aceder a "Gestão de Desafios" → "Novo Desafio"
   - Definir título, descrição e objetivos
   - Escolher nível de dificuldade
   - Atribuir pontos

2. **Acompanhar Alunos**
   - Ver lista de alunos no dashboard
   - Monitorizar atividade

### Para Administradores
1. **Gerir Sistema**
   - Visualizar estatísticas globais
   - Aceder a todos os utilizadores
   - Moderar desafios

## Desenvolvimento

### Compilar o Projeto

```bash
# Debug
dotnet build

# Release
dotnet build -c Release
```

### Executar Testes (quando implementados)

```bash
dotnet test
```

### Publicar para Produção

```bash
dotnet publish -c Release -o ./publish
```

## Integração com Base de Dados MySQL

### 1. Configurar Connection String

Editar `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=DEC;User=root;Password=sua_password;"
  }
}
```

### 2. Descomentar DbContext

Em `Program.cs`, descomentar:

```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);
```

### 3. Instalar Ferramentas EF Core

```bash
dotnet tool install --global dotnet-ef
```

### 4. Criar e Aplicar Migrations

```bash
# Criar migration inicial
dotnet ef migrations add InitialCreate

# Aplicar à base de dados
dotnet ef database update
```

## Alterações de Configuração

### Alterar Porta

Editar `Properties/launchSettings.json` ou usar variável de ambiente:

```bash
dotnet run --urls "https://localhost:7000;http://localhost:5500"
```

### Modo de Desenvolvimento

```bash
# Ativar hot reload
dotnet watch run
```

### Configurar HTTPS

```bash
# Criar certificado de desenvolvimento
dotnet dev-certs https --trust
```

## Resolução de Problemas

### Porta já em uso

```bash
# Encontrar processo na porta
lsof -i :5000

# Ou usar outra porta
dotnet run --urls "http://localhost:5500"
```

### Erros de certificado HTTPS

```bash
# Limpar e recriar certificados
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

### Erros de compilação

```bash
# Limpar build anterior
dotnet clean

# Restaurar pacotes
dotnet restore

# Compilar novamente
dotnet build
```

## Próximos Passos Recomendados

1. **Implementar Autenticação Real**
   - Adicionar ASP.NET Identity
   - Hash de passwords com BCrypt
   - Gestão de sessões seguras

2. **Integrar Base de Dados**
   - Configurar MySQL
   - Criar e aplicar migrations
   - Testar operações CRUD

3. **Adicionar Funcionalidades**
   - Sistema de badges
   - Gráficos interativos (Chart.js)
   - Relatórios em PDF
   - Exportação de dados

4. **Melhorias de UX**
   - Validação de formulários com JavaScript
   - Feedback visual (toasts, modals)
   - Responsividade mobile

5. **Testes**
   - Testes unitários (xUnit)
   - Testes de integração
   - Testes de UI (Selenium)

## Recursos Adicionais

- [Documentação ASP.NET Core](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [Razor Pages](https://docs.microsoft.com/aspnet/core/razor-pages)
- [MySQL Connector](https://dev.mysql.com/doc/connector-net/en/)

## Suporte

Para questões técnicas:
- Email: suporte.dec@istec.pt
- Documentação: Ver README.md
- Issues: Criar issue no repositório

---

**Desenvolvido para o ISTEC - Instituto Superior de Tecnologias Avançadas**
