using Microsoft.AspNetCore.Identity;

namespace HabitTracker.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Habit> Habits { get; set; } = new List<Habit>();
    }
}
