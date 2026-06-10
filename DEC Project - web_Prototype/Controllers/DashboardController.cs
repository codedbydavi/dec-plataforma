using Microsoft.AspNetCore.Mvc;
using DEC.Models;
using DEC.Services;

namespace DEC.Controllers
{
    public class DashboardController : Controller
    {
        private readonly MockDataService _dataService;

        public DashboardController()
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

            ViewBag.User = user;
            ViewBag.Stats = _dataService.GetDashboardStats(user.Id);

            if (user.Role == UserRole.Aluno)
            {
                ViewBag.Familias = _dataService.GetFamiliasByUserId(user.Id);
                ViewBag.Desafios = _dataService.GetAllDesafios();
                return View("AlunoIndex");
            }
            else if (user.Role == UserRole.Professor)
            {
                ViewBag.Desafios = _dataService.GetAllDesafios();
                ViewBag.Alunos = _dataService.GetAllUsers().Where(u => u.Role == UserRole.Aluno).ToList();
                return View("ProfessorIndex");
            }
            else if (user.Role == UserRole.Administrador)
            {
                ViewBag.Users = _dataService.GetAllUsers();
                return View("AdminIndex");
            }

            return View();
        }
    }
}
