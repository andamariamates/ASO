using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

namespace TodoApp.Data
{
    public class TodoDbContext : IdentityDbContext<IdentityUser>
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options)
            : base(options)
        {
        }
        public DbSet<TodoItem> TodoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TodoItem>().HasData(
                new TodoItem
                {
                    Id = 1,
                    Title = "Invata ASP.NET Core",
                    Description = "Parcurge documentatia oficiala Microsoft",
                    IsCompleted = false,
                    CreatedAt = new DateTime(2025, 1, 1)
                },
                new TodoItem
                {
                    Id = 2,
                    Title = "Fa primul proiect MVC",
                    Description = "Aplica cunostintele din laborator",
                    IsCompleted = true,
                    CreatedAt = new DateTime(2025, 1, 2)
                }
            );
        }
    }
}