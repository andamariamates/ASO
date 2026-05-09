using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;

namespace TodoApp.Controllers
{
    [Authorize(Roles = "Admin")] 
    public class AdminController : Controller
    {
        private readonly TodoDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(TodoDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var allUsers = await _userManager.Users.ToListAsync();
            var regularUsers = new List<UserStatsViewModel>();

            foreach (var user in allUsers)
            {
                if (await _userManager.IsInRoleAsync(user, "Admin")) continue;

                var tasks = await _context.TodoItems
                    .Where(t => t.UserId == user.Id)
                    .ToListAsync();

                regularUsers.Add(new UserStatsViewModel
                {
                    Username = user.UserName ?? "N/A",
                    TotalTasks = tasks.Count,
                    CompletedTasks = tasks.Count(t => t.IsCompleted),
                    ActiveTasks = tasks.Count(t => !t.IsCompleted)
                });
            }

            ViewBag.TotalUsers = regularUsers.Count;
            ViewBag.TotalTasks = regularUsers.Sum(u => u.TotalTasks);
            ViewBag.TotalCompleted = regularUsers.Sum(u => u.CompletedTasks);
            ViewBag.TotalActive = regularUsers.Sum(u => u.ActiveTasks);

            return View(regularUsers);
        }
    }

    public class UserStatsViewModel
    {
        public required string Username { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int ActiveTasks { get; set; }
    }
}