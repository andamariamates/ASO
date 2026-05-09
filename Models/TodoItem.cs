using Microsoft.AspNetCore.Identity;

namespace TodoApp.Models
{
    public class TodoItem
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public string? Description { get; set; }

        public bool IsCompleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? DueDate { get; set; }

        public string? UserId { get; set; } 

        public IdentityUser? User { get; set; } 
    }
}