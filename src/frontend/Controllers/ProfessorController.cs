using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Frontend.Data;
using Frontend.Models.Entities;
using Frontend.Models.ViewModels;
using System.Security.Claims;

namespace Frontend.Controllers
{
    [Authorize(Roles = "Professor")]
    public class ProfessorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProfessorController> _logger;

        public ProfessorController(ApplicationDbContext context, ILogger<ProfessorController> logger)
        {
            _context = context;
            _logger = logger;
        }

        private int GetUserId()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(userIdStr, out int userId);
            return userId;
        }

        public async Task<IActionResult> Dashboard()
        {
            int userId = GetUserId();
            if (userId == 0) return Unauthorized();

            var classes = await _context.Classrooms
                    .Include(c => c.Enrollments)
                        .ThenInclude(e => e.Student)
                            .ThenInclude(s => s!.Scenarios)
                                .ThenInclude(sc => sc.Histories)
                    .Where(c => c.TeacherId == userId)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToListAsync();

            var viewModel = new ProfessorDashboardViewModel
            {
                Classes = classes,
                GlobalChallenges = await _context.Challenges
                    .OrderByDescending(c => c.Id)
                    .ToListAsync()
            };

            // Calculate Chart Data
            var allEnrollments = classes.SelectMany(c => c.Enrollments).Where(e => e.Student != null).ToList();
            
            foreach (var enrollment in allEnrollments.Take(5)) // Show top 5 or first 5
            {
                var student = enrollment.Student!;
                viewModel.StudentNames.Add(student.FullName);
                
                var histories = student.Scenarios.SelectMany(s => s.Histories).ToList();
                viewModel.StudentChallenges.Add(histories.Count);
                
                float avgScore = histories.Any(h => h.Score.HasValue) 
                    ? (float)histories.Where(h => h.Score.HasValue).Average(h => h.Score!.Value) 
                    : 0;
                viewModel.StudentScores.Add(avgScore);
            }

            var allHistories = classes.SelectMany(c => c.Enrollments)
                .Where(e => e.Student != null)
                .SelectMany(e => e.Student!.Scenarios)
                .SelectMany(s => s.Histories)
                .ToList();

            viewModel.TotalEvaluated = allHistories.Count(h => h.Score.HasValue);
            viewModel.TotalPending = allHistories.Count(h => !h.Score.HasValue);

            return View(viewModel);
        }

        public async Task<IActionResult> Challenges()
        {
            int userId = GetUserId();
            var challenges = await _context.Challenges
                .OrderByDescending(c => c.Id)
                .ToListAsync();

            ViewBag.Classes = await _context.Classrooms
                .Where(c => c.TeacherId == userId)
                .ToListAsync();

            return View(challenges);
        }

        public async Task<IActionResult> ClassDetails(int id)
        {
            int userId = GetUserId();
            var classroom = await _context.Classrooms
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Student)
                        .ThenInclude(s => s!.Scenarios)
                            .ThenInclude(sc => sc.Histories)
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Student)
                        .ThenInclude(s => s!.Scenarios)
                            .ThenInclude(sc => sc.Challenge) // Carregar o desafio vinculado
                .FirstOrDefaultAsync(c => c.Id == id && c.TeacherId == userId);

            if (classroom == null) return NotFound();

            // Load assigned challenges
            ViewBag.AssignedChallenges = await _context.ChallengeAssignments
                .Include(a => a.Challenge)
                .Where(a => a.ClassroomId == id)
                .ToListAsync();

            return View(classroom);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateChallenge(CreateChallengeViewModel model)
        {
            // We ignore complex nested validation for simple creation
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.AccessLink) || string.IsNullOrEmpty(model.Description))
            {
                TempData["ErrorMessage"] = "Nome, Descrição e Link são obrigatórios.";
                return RedirectToAction("Dashboard");
            }

            var challenge = new Challenge
            {
                Name = model.Name,
                Description = model.Description,
                AccessLink = model.AccessLink,
                StatusId = 1 // Active
            };

            _context.Challenges.Add(challenge);
            await _context.SaveChangesAsync();

            // Auto-assign to class if selected
            if (model.TargetClassroomId.HasValue && model.TargetClassroomId.Value > 0)
            {
                var assignment = new ChallengeAssignment
                {
                    ChallengeId = challenge.Id,
                    ClassroomId = model.TargetClassroomId.Value
                };
                _context.ChallengeAssignments.Add(assignment);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Desafio '{model.Name}' criado e atribuído com sucesso!";
            }
            else
            {
                TempData["SuccessMessage"] = "Desafio pedagógico criado com sucesso!";
            }

            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignChallenge(AssignChallengeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Could not assign challenge.";
                return RedirectToAction("Dashboard");
            }

            var alreadyAssigned = await _context.ChallengeAssignments
                .AnyAsync(a => a.ChallengeId == model.ChallengeId && a.ClassroomId == model.ClassroomId);

            if (alreadyAssigned)
            {
                TempData["ErrorMessage"] = "Challenge already assigned to this class.";
                return RedirectToAction("Dashboard");
            }

            var assignment = new ChallengeAssignment
            {
                ChallengeId = model.ChallengeId,
                ClassroomId = model.ClassroomId
            };

            _context.ChallengeAssignments.Add(assignment);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Challenge assigned to class successfully!";
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Evaluate(int historyId, float score, string feedback)
        {
            var history = await _context.SimulationHistories.FindAsync(historyId);
            if (history == null) return NotFound();

            history.Score = score;
            history.Feedback = feedback;

            _context.SimulationHistories.Update(history);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Evaluation for student submitted successfully! Score: {score}";
            return RedirectToAction("Dashboard");
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

            int userId = GetUserId();
            var newClass = new Classroom
            {
                Name = name,
                TeacherId = userId,
                StatusId = 1,
                MemberCode = new Random().Next(100000, 999999)
            };

            _context.Classrooms.Add(newClass);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Class '{name}' created with code {newClass.MemberCode}.";
            return RedirectToAction("Dashboard");
        }
    }
}
