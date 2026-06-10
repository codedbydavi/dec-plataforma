using Microsoft.AspNetCore.Mvc;
using DEC.Models;
using DEC.Services;

namespace DEC.Controllers
{
    public class DesafioController : Controller
    {
        private readonly MockDataService _dataService;

        public DesafioController()
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

            var desafios = _dataService.GetAllDesafios();
            ViewBag.User = user;
            return View(desafios);
        }

        public IActionResult Create()
        {
            var user = GetCurrentUser();
            if (user == null || user.Role == UserRole.Aluno)
                return RedirectToAction("Login", "Home");

            return View();
        }

        [HttpPost]
        public IActionResult Create(Desafio desafio)
        {
            var user = GetCurrentUser();
            if (user == null || user.Role == UserRole.Aluno)
                return RedirectToAction("Login", "Home");

            desafio.CriadoPorId = user.Id;
            _dataService.AddDesafio(desafio);

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var user = GetCurrentUser();
            if (user == null || user.Role == UserRole.Aluno)
                return RedirectToAction("Login", "Home");

            var desafio = _dataService.GetDesafioById(id);
            if (desafio == null)
                return NotFound();

            return View(desafio);
        }

        [HttpPost]
        public IActionResult Edit(Desafio desafio)
        {
            var user = GetCurrentUser();
            if (user == null || user.Role == UserRole.Aluno)
                return RedirectToAction("Login", "Home");

            _dataService.UpdateDesafio(desafio);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var user = GetCurrentUser();
            if (user == null || user.Role == UserRole.Aluno)
                return RedirectToAction("Login", "Home");

            _dataService.DeleteDesafio(id);

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var user = GetCurrentUser();
            if (user == null)
                return RedirectToAction("Login", "Home");

            var desafio = _dataService.GetDesafioById(id);
            if (desafio == null)
                return NotFound();

            return View(desafio);
        }
    }
}
