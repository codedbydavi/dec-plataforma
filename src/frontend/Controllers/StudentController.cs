using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Frontend.Data;
using Frontend.Models;
using Frontend.Models.Entities;
using Frontend.Models.ViewModels;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();
            
            var enrollments = await _context.Enrollments
                .Include(e => e.ClassGroup)
                .ThenInclude(c => c!.Teacher)
                .Where(e => e.StudentId == userId)
                .ToListAsync();

            var scenarios = await _context.Scenarios
                .Where(s => s.StudentId == userId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            ViewBag.Classes = enrollments.Select(e => e.ClassGroup).ToList();
            ViewBag.Scenarios = scenarios;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> JoinClass(JoinClassViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid join code format.";
                return RedirectToAction("Dashboard");
            }

            if (!int.TryParse(model.JoinCode, out int memberCode))
            {
                TempData["ErrorMessage"] = "Join code must be a number.";
                return RedirectToAction("Dashboard");
            }

            var classroom = await _context.Classrooms
                .FirstOrDefaultAsync(c => c.MemberCode == memberCode);

            if (classroom == null)
            {
                TempData["ErrorMessage"] = "Class not found.";
                return RedirectToAction("Dashboard");
            }

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();
            
            var alreadyEnrolled = await _context.Enrollments
                .AnyAsync(e => e.StudentId == userId && e.ClassGroupId == classroom.Id);

            if (alreadyEnrolled)
            {
                TempData["ErrorMessage"] = "You are already enrolled in this class.";
                return RedirectToAction("Dashboard");
            }

            var enrollment = new Enrollment
            {
                StudentId = userId,
                ClassGroupId = classroom.Id
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Successfully joined the class!";
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateScenario(CreateScenarioViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid scenario data.";
                return RedirectToAction("Dashboard");
            }

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();
            
            var scenario = new Scenario
            {
                StudentId = userId,
                FamilyName = model.FamilyName,
                InitialBalance = model.InitialBalance
            };

            _context.Scenarios.Add(scenario);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Scenario created successfully!";
            return RedirectToAction("Dashboard");
        }

        public async Task<IActionResult> ScenarioDetails(int id)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();

            var scenario = await _context.Scenarios
                .Include(s => s.Entries)
                .ThenInclude(e => e.EntryType)
                .Include(s => s.Entries)
                .ThenInclude(e => e.Category)
                .Include(s => s.Objectives)
                .Include(s => s.Histories)
                .FirstOrDefaultAsync(s => s.Id == id && s.StudentId == userId);

            if (scenario == null) return NotFound();

            return View(scenario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEntry(AddEntryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid entry data.";
                return RedirectToAction("ScenarioDetails", new { id = model.ScenarioId });
            }

            // Logic to decide between Income or Expense based on TypeId or some logic
            // For now, let's assume we use EntryType lookup to decide
            var entryType = await _context.EntryTypes.FindAsync(model.TypeId);
            FinancialEntry entry;

            if (entryType?.Type == "INCOME")
            {
                entry = new Income();
            }
            else
            {
                entry = new Expense();
            }

            entry.ScenarioId = model.ScenarioId;
            entry.TypeId = model.TypeId;
            entry.CategoryId = model.CategoryId;
            entry.Amount = model.Amount;
            entry.Month = model.Month;
            entry.Recurrence = model.Recurrence;

            _context.FinancialEntries.Add(entry);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Entry added successfully!";
            return RedirectToAction("ScenarioDetails", new { id = model.ScenarioId });
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddObjective(AddObjectiveViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid objective data.";
                return RedirectToAction("ScenarioDetails", new { id = model.ScenarioId });
            }

            var objective = new Objective
            {
                ScenarioId = model.ScenarioId,
                Description = model.Description,
                TargetValue = model.TargetValue,
                TargetMonths = model.TargetMonths
            };

            _context.Objectives.Add(objective);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Objective added successfully!";
            return RedirectToAction("ScenarioDetails", new { id = model.ScenarioId });
        }
    }
}
