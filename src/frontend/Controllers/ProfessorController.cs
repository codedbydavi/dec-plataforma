using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Frontend.Data;
using Frontend.Models.Entities;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Frontend.Controllers
{
    [Authorize]
    public class ProfessorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfessorController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();

            var classes = await _context.Classrooms
                .Where(c => c.TeacherId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return View(classes);
        }

        public async Task<IActionResult> ClassDetails(int id)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();

            var classroom = await _context.Classrooms
                .Include(c => c.Enrollments)
                .ThenInclude(e => e.Student)
                .FirstOrDefaultAsync(c => c.Id == id && c.TeacherId == userId);

            if (classroom == null) return NotFound();

            return View(classroom);
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

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();
            
            var newClass = new Classroom
            {
                Name = name,
                TeacherId = userId,
                MemberCode = new Random().Next(100000, 999999)
            };

            _context.Classrooms.Add(newClass);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Class created successfully!";
            return RedirectToAction("Dashboard");
        }
    }
}
