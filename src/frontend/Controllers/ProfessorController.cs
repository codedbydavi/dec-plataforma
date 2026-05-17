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
            var turmas = await _educationService.GetMinhasTurmasAsync();
            return View(turmas);
        }

        public async Task<IActionResult> TurmaDetalhes(int id)
        {
            var turma = await _educationService.GetTurmaDetalhesAsync(id);
            if (turma == null) return NotFound();
            
            return View(turma);
        }
    }
}
