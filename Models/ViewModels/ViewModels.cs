using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Full name is required")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please confirm your password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }

    public class HabitFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Habit name is required")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [Display(Name = "Habit Name")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(300, ErrorMessage = "Description cannot exceed 300 characters")]
        public string Description { get; set; } = string.Empty;

        public string Frequency { get; set; } = "Daily";
    }

    public class DashboardViewModel
    {
        public List<Habit> Habits { get; set; } = new();
        public int TotalHabits { get; set; }
        public int CompletedToday { get; set; }
        public int TotalCompletionsAllTime { get; set; }
        public int BestStreak { get; set; }
        public int CurrentStreak { get; set; }
        public string UserName { get; set; } = string.Empty;
    }

    public class StatisticsViewModel
    {
        public double CompletionRate { get; set; }
        public int BestStreak { get; set; }
        public int CurrentStreak { get; set; }
        public int TotalCompletions { get; set; }
        public int TotalHabits { get; set; }
        public int CompletedToday { get; set; }
        public List<int> Last7DaysCounts { get; set; } = new();
        public List<string> Last7DaysLabels { get; set; } = new();
        public List<HabitStat> HabitStats { get; set; } = new();
        public List<int> HeatmapData { get; set; } = new();
    }

    public class HabitStat
    {
        public string Name { get; set; } = string.Empty;
        public int CompletionPct { get; set; }
        public int CurrentStreak { get; set; }
        public int TotalCompletions { get; set; }
    }
}
