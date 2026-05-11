using System.ComponentModel.DataAnnotations.Schema;

namespace HabitTracker.Models
{
    public class HabitLog
    {
        public int Id { get; set; }

        public int HabitId { get; set; }

        [ForeignKey("HabitId")]
        public Habit? Habit { get; set; }

        public DateTime CompletedDate { get; set; } = DateTime.UtcNow.Date;

        [System.ComponentModel.DataAnnotations.MaxLength(300)]
        public string? Notes { get; set; }
    }
}
