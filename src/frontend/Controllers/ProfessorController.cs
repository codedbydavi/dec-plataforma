using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Frontend.Services;

namespace Frontend.Controllers
{
    [Authorize(Roles = "TEACHER,ADMIN")]
    public class ProfessorController : Controller
    {
        private readonly IEducationService _educationService;

        public ProfessorController(IEducationService educationService)
        {
            _educationService = educationService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var turmas = await _educationService.GetMyClassesAsync();
            return View(turmas);
        }

        public async Task<IActionResult> ClassDetails(int id)
        {
            var turma = await _educationService.GetClassDetailsAsync(id);
            if (turma == null) return NotFound();

            return View(turma);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateClass(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                TempData["ErrorMessage"] = "Class name cannot be empty.";
                return RedirectToAction("Dashboard");
            }

            var success = await _educationService.CreateClassAsync(name);
            if (success)
            {
                TempData["SuccessMessage"] = "Class created successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create class.";
            }

            return RedirectToAction("Dashboard");
        }
    }
}

