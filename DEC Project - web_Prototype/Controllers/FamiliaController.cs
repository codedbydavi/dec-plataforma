using Microsoft.AspNetCore.Mvc;
using DEC.Models;
using DEC.Services;

namespace DEC.Controllers
{
    public class FamiliaController : Controller
    {
        private readonly MockDataService _dataService;

        public FamiliaController()
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

        public IActionResult Index()
        {
            var user = GetCurrentUser();
            if (user == null)
                return RedirectToAction("Login", "Home");

            var familias = _dataService.GetFamiliasByUserId(user.Id);
            return View(familias);
        }

        public IActionResult Create()
        {
            var user = GetCurrentUser();
            if (user == null)
                return RedirectToAction("Login", "Home");

            return View();
        }

        [HttpPost]
        public IActionResult Create(Familia familia)
        {
            var user = GetCurrentUser();
            if (user == null)
                return RedirectToAction("Login", "Home");

            familia.UserId = user.Id;
            _dataService.AddFamilia(familia);

            return RedirectToAction("Details", new { id = familia.Id });
        }

        public IActionResult Details(int id)
        {
            var user = GetCurrentUser();
            if (user == null)
                return RedirectToAction("Login", "Home");

            var familia = _dataService.GetFamiliaById(id);
            if (familia == null || familia.UserId != user.Id)
                return NotFound();

            ViewBag.Transacoes = _dataService.GetTransacoesByFamiliaId(id);
            return View(familia);
        }

        public IActionResult Edit(int id)
        {
            var user = GetCurrentUser();
            if (user == null)
                return RedirectToAction("Login", "Home");

            var familia = _dataService.GetFamiliaById(id);
            if (familia == null || familia.UserId != user.Id)
                return NotFound();

            return View(familia);
        }

        [HttpPost]
        public IActionResult Edit(Familia familia)
        {
            var user = GetCurrentUser();
            if (user == null)
                return RedirectToAction("Login", "Home");

            var existing = _dataService.GetFamiliaById(familia.Id);
            if (existing == null || existing.UserId != user.Id)
                return NotFound();

            familia.UserId = user.Id;
            _dataService.UpdateFamilia(familia);

            return RedirectToAction("Details", new { id = familia.Id });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var user = GetCurrentUser();
            if (user == null)
                return RedirectToAction("Login", "Home");

            var familia = _dataService.GetFamiliaById(id);
            if (familia == null || familia.UserId != user.Id)
                return NotFound();

            _dataService.DeleteFamilia(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddTransacao(int familiaId, Transacao transacao)
        {
            var user = GetCurrentUser();
            if (user == null)
                return RedirectToAction("Login", "Home");

            var familia = _dataService.GetFamiliaById(familiaId);
            if (familia == null || familia.UserId != user.Id)
                return NotFound();

            transacao.FamiliaId = familiaId;
            _dataService.AddTransacao(transacao);

            return RedirectToAction("Details", new { id = familiaId });
        }

        [HttpPost]
        public IActionResult DeleteTransacao(int id, int familiaId)
        {
            var user = GetCurrentUser();
            if (user == null)
                return RedirectToAction("Login", "Home");

            var familia = _dataService.GetFamiliaById(familiaId);
            if (familia == null || familia.UserId != user.Id)
                return NotFound();

            _dataService.DeleteTransacao(id);

            return RedirectToAction("Details", new { id = familiaId });
        }
    }
}
