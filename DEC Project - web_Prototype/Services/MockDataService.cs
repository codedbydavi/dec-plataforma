using DEC.Models;

namespace DEC.Services
{
    public class MockDataService
    {
        private static List<User> _users = new()
        {
            new User { Id = 1, Nome = "Admin ISTEC", Email = "admin@istec.pt", Password = "admin123", Role = UserRole.Administrador, DataCriacao = DateTime.Now },
            new User { Id = 2, Nome = "Prof. Silva", Email = "prof.silva@istec.pt", Password = "prof123", Role = UserRole.Professor, DataCriacao = DateTime.Now },
            new User { Id = 3, Nome = "João Aluno", Email = "joao@istec.pt", Password = "aluno123", Role = UserRole.Aluno, Turma = "10A", DataCriacao = DateTime.Now },
            new User { Id = 4, Nome = "Maria Costa", Email = "maria@istec.pt", Password = "aluno123", Role = UserRole.Aluno, Turma = "11B", DataCriacao = DateTime.Now }
        };

        private static List<Familia> _familias = new()
        {
            new Familia { Id = 1, UserId = 3, Nome = "Família Silva", Descricao = "Casal com 2 filhos", NumeroAdultos = 2, NumeroCriancas = 2, RendimentoMensal = 2500m, DespesasFixas = 1200m, DespesasVariaveis = 600m, Poupanca = 700m, TaxaEsforco = 48m, Ativa = true },
            new Familia { Id = 2, UserId = 3, Nome = "Família Costa", Descricao = "Mãe solteira com 1 filho", NumeroAdultos = 1, NumeroCriancas = 1, RendimentoMensal = 1500m, DespesasFixas = 800m, DespesasVariaveis = 400m, Poupanca = 300m, TaxaEsforco = 53m, Ativa = false }
        };

        private static List<Transacao> _transacoes = new()
        {
            new Transacao { Id = 1, FamiliaId = 1, Descricao = "Salário Principal", Valor = 1800m, Tipo = TipoTransacao.Rendimento, Categoria = CategoriaTransacao.Salario, Data = DateTime.Now.AddDays(-5), Recorrente = true },
            new Transacao { Id = 2, FamiliaId = 1, Descricao = "Salário Secundário", Valor = 700m, Tipo = TipoTransacao.Rendimento, Categoria = CategoriaTransacao.Salario, Data = DateTime.Now.AddDays(-5), Recorrente = true },
            new Transacao { Id = 3, FamiliaId = 1, Descricao = "Renda", Valor = 650m, Tipo = TipoTransacao.DespesaFixa, Categoria = CategoriaTransacao.Habitacao, Data = DateTime.Now.AddDays(-3), Recorrente = true },
            new Transacao { Id = 4, FamiliaId = 1, Descricao = "Supermercado", Valor = 250m, Tipo = TipoTransacao.DespesaVariavel, Categoria = CategoriaTransacao.Alimentacao, Data = DateTime.Now.AddDays(-2), Recorrente = false }
        };

        private static List<Desafio> _desafios = new()
        {
            new Desafio { Id = 1, CriadoPorId = 2, Titulo = "Orçamento Familiar Básico", Descricao = "Crie uma família e registe todas as suas receitas e despesas mensais", Nivel = NivelDificuldade.Facil, Pontos = 50, ObjetivoAprendizagem = "Compreender conceitos básicos de orçamento familiar", Ativo = true },
            new Desafio { Id = 2, CriadoPorId = 2, Titulo = "Taxa de Esforço Ideal", Descricao = "Ajuste as despesas da sua família para atingir uma taxa de esforço inferior a 35%", Nivel = NivelDificuldade.Medio, Pontos = 100, ObjetivoAprendizagem = "Entender a importância da taxa de esforço na gestão financeira", Ativo = true },
            new Desafio { Id = 3, CriadoPorId = 2, Titulo = "Plano de Poupança", Descricao = "Simule uma poupança de 500€ por mês durante 5 anos com juros compostos", Nivel = NivelDificuldade.Dificil, Pontos = 150, ObjetivoAprendizagem = "Aplicar conceitos de juros compostos e planeamento a longo prazo", Ativo = true }
        };

        private static int _userIdCounter = 5;
        private static int _familiaIdCounter = 3;
        private static int _transacaoIdCounter = 5;
        private static int _simulacaoIdCounter = 1;
        private static int _desafioIdCounter = 4;

        public List<User> GetAllUsers() => _users;
        public User? GetUserById(int id) => _users.FirstOrDefault(u => u.Id == id);
        public User? GetUserByEmail(string email) => _users.FirstOrDefault(u => u.Email == email);
        public User? ValidateUser(string email, string password) => _users.FirstOrDefault(u => u.Email == email && u.Password == password);

        public void AddUser(User user)
        {
            user.Id = _userIdCounter++;
            user.DataCriacao = DateTime.Now;
            _users.Add(user);
        }

        public List<Familia> GetFamiliasByUserId(int userId) => _familias.Where(f => f.UserId == userId).ToList();
        public Familia? GetFamiliaById(int id) => _familias.FirstOrDefault(f => f.Id == id);

        public void AddFamilia(Familia familia)
        {
            familia.Id = _familiaIdCounter++;
            familia.DataCriacao = DateTime.Now;
            _familias.Add(familia);
        }

        public void UpdateFamilia(Familia familia)
        {
            var existing = _familias.FirstOrDefault(f => f.Id == familia.Id);
            if (existing != null)
            {
                _familias.Remove(existing);
                _familias.Add(familia);
            }
        }

        public void DeleteFamilia(int id)
        {
            var familia = _familias.FirstOrDefault(f => f.Id == id);
            if (familia != null)
            {
                _familias.Remove(familia);
                _transacoes.RemoveAll(t => t.FamiliaId == id);
            }
        }

        public List<Transacao> GetTransacoesByFamiliaId(int familiaId) => _transacoes.Where(t => t.FamiliaId == familiaId).OrderByDescending(t => t.Data).ToList();

        public void AddTransacao(Transacao transacao)
        {
            transacao.Id = _transacaoIdCounter++;
            _transacoes.Add(transacao);

            // Atualizar totais da família
            var familia = GetFamiliaById(transacao.FamiliaId);
            if (familia != null)
            {
                RecalcularTotaisFamilia(familia.Id);
            }
        }

        public void DeleteTransacao(int id)
        {
            var transacao = _transacoes.FirstOrDefault(t => t.Id == id);
            if (transacao != null)
            {
                var familiaId = transacao.FamiliaId;
                _transacoes.Remove(transacao);
                RecalcularTotaisFamilia(familiaId);
            }
        }

        private void RecalcularTotaisFamilia(int familiaId)
        {
            var familia = GetFamiliaById(familiaId);
            if (familia != null)
            {
                var transacoes = GetTransacoesByFamiliaId(familiaId);

                familia.RendimentoMensal = transacoes.Where(t => t.Tipo == TipoTransacao.Rendimento).Sum(t => t.Valor);
                familia.DespesasFixas = transacoes.Where(t => t.Tipo == TipoTransacao.DespesaFixa).Sum(t => t.Valor);
                familia.DespesasVariaveis = transacoes.Where(t => t.Tipo == TipoTransacao.DespesaVariavel).Sum(t => t.Valor);
                familia.Poupanca = transacoes.Where(t => t.Tipo == TipoTransacao.Poupanca).Sum(t => t.Valor);

                var despesasHabitacao = transacoes
                    .Where(t => t.Categoria == CategoriaTransacao.Habitacao)
                    .Sum(t => t.Valor);

                familia.TaxaEsforco = familia.RendimentoMensal > 0
                    ? Math.Round((despesasHabitacao / familia.RendimentoMensal) * 100, 2)
                    : 0;
            }
        }

        public List<Desafio> GetAllDesafios() => _desafios.Where(d => d.Ativo).ToList();
        public Desafio? GetDesafioById(int id) => _desafios.FirstOrDefault(d => d.Id == id);

        public void AddDesafio(Desafio desafio)
        {
            desafio.Id = _desafioIdCounter++;
            desafio.DataCriacao = DateTime.Now;
            _desafios.Add(desafio);
        }

        public void UpdateDesafio(Desafio desafio)
        {
            var existing = _desafios.FirstOrDefault(d => d.Id == desafio.Id);
            if (existing != null)
            {
                _desafios.Remove(existing);
                _desafios.Add(desafio);
            }
        }

        public void DeleteDesafio(int id)
        {
            var desafio = _desafios.FirstOrDefault(d => d.Id == id);
            if (desafio != null)
            {
                desafio.Ativo = false;
            }
        }

        public Dictionary<string, object> GetDashboardStats(int userId)
        {
            var user = GetUserById(userId);
            if (user == null) return new Dictionary<string, object>();

            var stats = new Dictionary<string, object>();

            if (user.Role == UserRole.Administrador)
            {
                stats["totalUsers"] = _users.Count;
                stats["totalAlunos"] = _users.Count(u => u.Role == UserRole.Aluno);
                stats["totalProfessores"] = _users.Count(u => u.Role == UserRole.Professor);
                stats["totalFamilias"] = _familias.Count;
                stats["totalDesafios"] = _desafios.Count(d => d.Ativo);
            }
            else if (user.Role == UserRole.Professor)
            {
                stats["totalAlunos"] = _users.Count(u => u.Role == UserRole.Aluno);
                stats["totalDesafiosCriados"] = _desafios.Count(d => d.CriadoPorId == userId && d.Ativo);
                stats["totalFamilias"] = _familias.Count;
            }
            else if (user.Role == UserRole.Aluno)
            {
                var familias = GetFamiliasByUserId(userId);
                stats["totalFamilias"] = familias.Count;
                stats["familiasAtivas"] = familias.Count(f => f.Ativa);
                stats["totalDesafios"] = _desafios.Count(d => d.Ativo);

                if (familias.Any())
                {
                    stats["taxaEsforcoMedia"] = Math.Round(familias.Average(f => (double)f.TaxaEsforco), 2);
                }
            }

            return stats;
        }
    }
}
