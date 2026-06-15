using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Frontend.Data;
using Frontend.Models.ViewModels;

namespace Frontend.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            var labels = new string[7];
            var scenariosData = new int[7];
            var simulationsData = new int[7];
            var today = DateTime.UtcNow.Date;

            for (int i = 0; i < 7; i++)
            {
                var date = today.AddDays(-6 + i);
                labels[i] = date.ToString("dd/MM");

                scenariosData[i] = await _context.Scenarios
                    .CountAsync(s => s.CreatedAt.Date == date);

                simulationsData[i] = await _context.SimulationHistories
                    .CountAsync(s => s.ExecutionDate.Date == date);
            }

            var viewModel = new AdminDashboardViewModel
            {
                Labels = labels,
                ScenariosData = scenariosData,
                SimulationsData = simulationsData,
                TotalUsers = await _context.Users.CountAsync(),
                TotalScenarios = await _context.Scenarios.CountAsync(),
                TotalSimulations = await _context.SimulationHistories.CountAsync(),
                ActiveClasses = await _context.Classrooms.CountAsync(c => c.StatusId == 1)
            };

            return View(viewModel);
        }
    }
}
