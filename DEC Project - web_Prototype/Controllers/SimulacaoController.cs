using Microsoft.AspNetCore.Mvc;
using DEC.Models;
using DEC.Services;

namespace DEC.Controllers
{
    public class SimulacaoController : Controller
    {
        private readonly MockDataService _dataService;

        public SimulacaoController()
        {
            _dataService = new MockDataService();
        }

        private User? GetCurrentUser()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
                return null;
            return _dataService.GetUserById(userId.Value);
        }

        public IActionResult Index(int familiaId)
        {
            var user = GetCurrentUser();
            if (user == null)
                return RedirectToAction("Login", "Home");

            var familia = _dataService.GetFamiliaById(familiaId);
            if (familia == null || familia.UserId != user.Id)
                return NotFound();

            ViewBag.Familia = familia;
            return View();
        }

        [HttpPost]
        public IActionResult CalcularJurosCompostos(int familiaId, decimal valorInicial, decimal taxaJuros, int periodo, decimal contribuicaoMensal = 0)
        {
            var resultado = new
            {
                valorInicial = valorInicial,
                taxaJuros = taxaJuros,
                periodo = periodo,
                contribuicaoMensal = contribuicaoMensal,
                valorFinal = CalcularJurosCompostosComContribuicoes(valorInicial, taxaJuros, periodo, contribuicaoMensal),
                detalhes = GerarDetalhesJurosCompostos(valorInicial, taxaJuros, periodo, contribuicaoMensal)
            };

            return Json(resultado);
        }

        [HttpPost]
        public IActionResult CalcularAmortizacao(int familiaId, decimal valorEmprestimo, decimal taxaJuros, int periodo)
        {
            var resultado = new
            {
                valorEmprestimo = valorEmprestimo,
                taxaJuros = taxaJuros,
                periodo = periodo,
                prestacaoMensal = CalcularPrestacaoMensal(valorEmprestimo, taxaJuros, periodo),
                totalPago = CalcularTotalPago(valorEmprestimo, taxaJuros, periodo),
                totalJuros = CalcularTotalJuros(valorEmprestimo, taxaJuros, periodo),
                planoAmortizacao = GerarPlanoAmortizacao(valorEmprestimo, taxaJuros, periodo)
            };

            return Json(resultado);
        }

        [HttpPost]
        public IActionResult ProjetarInflacao(int familiaId, decimal valorAtual, decimal taxaInflacao, int anos)
        {
            var resultado = new
            {
                valorAtual = valorAtual,
                taxaInflacao = taxaInflacao,
                anos = anos,
                valorFuturo = CalcularValorComInflacao(valorAtual, taxaInflacao, anos),
                perdasPorAno = GerarPerdasPorAno(valorAtual, taxaInflacao, anos)
            };

            return Json(resultado);
        }

        private decimal CalcularJurosCompostosComContribuicoes(decimal valorInicial, decimal taxaJuros, int meses, decimal contribuicaoMensal)
        {
            decimal taxaMensal = taxaJuros / 100 / 12;
            decimal montante = valorInicial;

            for (int i = 0; i < meses; i++)
            {
                montante = (montante + contribuicaoMensal) * (1 + taxaMensal);
            }

            return Math.Round(montante, 2);
        }

        private List<object> GerarDetalhesJurosCompostos(decimal valorInicial, decimal taxaJuros, int meses, decimal contribuicaoMensal)
        {
            decimal taxaMensal = taxaJuros / 100 / 12;
            decimal montante = valorInicial;
            var detalhes = new List<object>();

            for (int i = 1; i <= meses; i++)
            {
                decimal juros = montante * taxaMensal;
                montante += juros + contribuicaoMensal;

                if (i % 12 == 0 || i == meses)
                {
                    detalhes.Add(new
                    {
                        mes = i,
                        montante = Math.Round(montante, 2),
                        totalContribuido = Math.Round(valorInicial + (contribuicaoMensal * i), 2),
                        totalJuros = Math.Round(montante - valorInicial - (contribuicaoMensal * i), 2)
                    });
                }
            }

            return detalhes;
        }

        private decimal CalcularPrestacaoMensal(decimal valorEmprestimo, decimal taxaJuros, int meses)
        {
            decimal taxaMensal = taxaJuros / 100 / 12;
            if (taxaMensal == 0) return valorEmprestimo / meses;

            decimal prestacao = valorEmprestimo * (taxaMensal * (decimal)Math.Pow((double)(1 + taxaMensal), meses)) /
                               ((decimal)Math.Pow((double)(1 + taxaMensal), meses) - 1);

            return Math.Round(prestacao, 2);
        }

        private decimal CalcularTotalPago(decimal valorEmprestimo, decimal taxaJuros, int meses)
        {
            return Math.Round(CalcularPrestacaoMensal(valorEmprestimo, taxaJuros, meses) * meses, 2);
        }

        private decimal CalcularTotalJuros(decimal valorEmprestimo, decimal taxaJuros, int meses)
        {
            return Math.Round(CalcularTotalPago(valorEmprestimo, taxaJuros, meses) - valorEmprestimo, 2);
        }

        private List<object> GerarPlanoAmortizacao(decimal valorEmprestimo, decimal taxaJuros, int meses)
        {
            decimal taxaMensal = taxaJuros / 100 / 12;
            decimal prestacao = CalcularPrestacaoMensal(valorEmprestimo, taxaJuros, meses);
            decimal saldoDevedor = valorEmprestimo;
            var plano = new List<object>();

            for (int i = 1; i <= Math.Min(meses, 12); i++)
            {
                decimal juros = saldoDevedor * taxaMensal;
                decimal amortizacao = prestacao - juros;
                saldoDevedor -= amortizacao;

                plano.Add(new
                {
                    mes = i,
                    prestacao = Math.Round(prestacao, 2),
                    juros = Math.Round(juros, 2),
                    amortizacao = Math.Round(amortizacao, 2),
                    saldoDevedor = Math.Round(Math.Max(saldoDevedor, 0), 2)
                });
            }

            return plano;
        }

        private decimal CalcularValorComInflacao(decimal valorAtual, decimal taxaInflacao, int anos)
        {
            return Math.Round(valorAtual / (decimal)Math.Pow((double)(1 + taxaInflacao / 100), anos), 2);
        }

        private List<object> GerarPerdasPorAno(decimal valorAtual, decimal taxaInflacao, int anos)
        {
            var perdas = new List<object>();

            for (int i = 1; i <= anos; i++)
            {
                decimal valorFuturo = CalcularValorComInflacao(valorAtual, taxaInflacao, i);
                decimal perda = valorAtual - valorFuturo;

                perdas.Add(new
                {
                    ano = i,
                    valorReal = Math.Round(valorFuturo, 2),
                    perda = Math.Round(perda, 2),
                    percentualPerda = Math.Round((perda / valorAtual) * 100, 2)
                });
            }

            return perdas;
        }
    }
}
