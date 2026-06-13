using Microsoft.AspNetCore.Mvc;
using DEC.Models;
using DEC.Services;
using Newtonsoft.Json;

namespace DEC.Controllers
{
    public class HomeController : Controller
    {
        private readonly MockDataService _dataService;

        public HomeController()
        {
            _dataService = new MockDataService();
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue)
            {
                var user = _dataService.GetUserById(userId.Value);
                if (user != null)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _dataService.ValidateUser(email, password);
            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserName", user.Nome);
                HttpContext.Session.SetString("UserRole", user.Role.ToString());

                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.Error = "Email ou password incorretos";
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (string.IsNullOrEmpty(user.Nome) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                ViewBag.Error = "Todos os campos são obrigatórios";
                return View(user);
            }

            var existingUser = _dataService.GetUserByEmail(user.Email);
            if (existingUser != null)
            {
                ViewBag.Error = "Este email já está registado";
                return View(user);
            }

            user.Role = UserRole.Aluno; // Default role
            _dataService.AddUser(user);

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Nome);
            HttpContext.Session.SetString("UserRole", user.Role.ToString());

            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
