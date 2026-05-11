using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitTracker.Models
{
    public class Habit
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(300)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Frequency { get; set; } = "Daily";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        public ICollection<HabitLog> Logs { get; set; } = new List<HabitLog>();

        [NotMapped]
        public int CurrentStreak { get; set; }

        [NotMapped]
        public int LongestStreak { get; set; }

        [NotMapped]
        public bool CompletedToday { get; set; }

        [NotMapped]
        public int TotalCompletions { get; set; }
    }
}
