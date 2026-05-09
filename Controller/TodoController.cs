using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Controllers
{
    [Authorize] 
    public class TodoController : Controller
    {
        private readonly TodoDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TodoController(TodoDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string? filter)
        {
            var userId = _userManager.GetUserId(User);

            var query = _context.TodoItems
                .Where(t => t.UserId == userId)
                .AsQueryable();

            query = filter switch
            {
                "active"    => query.Where(t => !t.IsCompleted),
                "completed" => query.Where(t => t.IsCompleted),
                _           => query
            };

            query = query.OrderBy(t => t.IsCompleted).ThenByDescending(t => t.CreatedAt);

            var todos = await query.ToListAsync();

            ViewBag.CurrentFilter = filter ?? "all";
            ViewBag.TotalCount     = await _context.TodoItems.CountAsync(t => t.UserId == userId);
            ViewBag.CompletedCount = await _context.TodoItems.CountAsync(t => t.UserId == userId && t.IsCompleted);
            ViewBag.ActiveCount    = await _context.TodoItems.CountAsync(t => t.UserId == userId && !t.IsCompleted);

            return View(todos);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TodoItem todoItem)
        {
            if (ModelState.IsValid)
            {
                todoItem.CreatedAt = DateTime.Now;
                todoItem.IsCompleted = false;
                todoItem.UserId = _userManager.GetUserId(User); // Asociem cu userul curent
                _context.Add(todoItem);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Sarcina a fost adaugata cu succes!";
                return RedirectToAction(nameof(Index));
            }
            return View(todoItem);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var userId = _userManager.GetUserId(User);
            // Verificam ca userul sa editeze doar sarcinile LUI
            var todoItem = await _context.TodoItems
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (todoItem == null) return NotFound();
            return View(todoItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TodoItem todoItem)
        {
            if (id != todoItem.Id) return NotFound();
            if (ModelState.IsValid)
            {
                todoItem.UserId = _userManager.GetUserId(User);
                _context.Update(todoItem);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Sarcina a fost actualizata!";
                return RedirectToAction(nameof(Index));
            }
            return View(todoItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            var todoItem = await _context.TodoItems
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (todoItem != null)
            {
                _context.TodoItems.Remove(todoItem);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Sarcina a fost stearsa!";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Toggle(int id)
        {
            var userId = _userManager.GetUserId(User);
            var todoItem = await _context.TodoItems
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (todoItem == null) return NotFound();
            todoItem.IsCompleted = !todoItem.IsCompleted;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}