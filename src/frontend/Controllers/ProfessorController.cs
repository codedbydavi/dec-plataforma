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

            var viewModel = new ProfessorDashboardViewModel
            {
                Classes = await _context.Classrooms
                    .Include(c => c.Enrollments)
                    .Where(c => c.TeacherId == userId)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToListAsync(),
                GlobalChallenges = await _context.Challenges
                    .OrderByDescending(c => c.Id)
                    .ToListAsync()
            };

            return View(viewModel);
        }

        public async Task<IActionResult> ClassDetails(int id)
        {
            int userId = GetUserId();
            var classroom = await _context.Classrooms
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Student)
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Student)
                    .ThenInclude(s => s!.Scenarios)
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
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid challenge data.";
                return RedirectToAction("Dashboard");
            }

            var challenge = new Challenge
            {
                Name = model.Name,
                AccessLink = model.AccessLink,
                StatusId = 1 // Active
            };

            _context.Challenges.Add(challenge);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Pedagogical challenge created successfully!";
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
