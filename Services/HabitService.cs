using HabitTracker.Data;
using HabitTracker.Models;
using HabitTracker.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Services
{
    public class HabitService
    {
        private readonly ApplicationDbContext _context;

        public HabitService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Habit>> GetUserHabitsAsync(string userId)
        {
            var habits = await _context.Habits
                .Where(h => h.UserId == userId && h.IsActive)
                .Include(h => h.Logs)
                .OrderBy(h => h.CreatedAt)
                .ToListAsync();

            foreach (var habit in habits)
                EnrichHabit(habit);

            return habits;
        }

        public async Task<Habit?> GetHabitByIdAsync(int id, string userId)
        {
            var habit = await _context.Habits
                .Include(h => h.Logs)
                .FirstOrDefaultAsync(h => h.Id == id && h.UserId == userId && h.IsActive);

            if (habit != null) EnrichHabit(habit);
            return habit;
        }

        public async Task<Habit> CreateHabitAsync(Habit habit)
        {
            habit.CreatedAt = DateTime.UtcNow;
            _context.Habits.Add(habit);
            await _context.SaveChangesAsync();
            return habit;
        }

        public async Task<bool> UpdateHabitAsync(Habit updated, string userId)
        {
            var habit = await _context.Habits
                .FirstOrDefaultAsync(h => h.Id == updated.Id && h.UserId == userId);
            if (habit == null) return false;

            habit.Name = updated.Name;
            habit.Description = updated.Description;
            habit.Frequency = updated.Frequency;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteHabitAsync(int id, string userId)
        {
            var habit = await _context.Habits
                .FirstOrDefaultAsync(h => h.Id == id && h.UserId == userId);
            if (habit == null) return false;

            habit.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ToggleCompletionAsync(int habitId, string userId)
        {
            var habit = await _context.Habits
                .Include(h => h.Logs)
                .FirstOrDefaultAsync(h => h.Id == habitId && h.UserId == userId);
            if (habit == null) return false;

            var today = DateTime.UtcNow.Date;
            var existing = habit.Logs.FirstOrDefault(l => l.CompletedDate.Date == today);

            if (existing != null)
                _context.HabitLogs.Remove(existing);
            else
                _context.HabitLogs.Add(new HabitLog { HabitId = habitId, CompletedDate = today });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<StatisticsViewModel> GetStatisticsAsync(string userId)
        {
            var habits = await GetUserHabitsAsync(userId);
            var today = DateTime.UtcNow.Date;

            var last7Labels = Enumerable.Range(0, 7)
                .Select(i => today.AddDays(-6 + i).ToString("ddd"))
                .ToList();

            var last7Counts = Enumerable.Range(0, 7)
                .Select(i =>
                {
                    var d = today.AddDays(-6 + i);
                    return habits.Sum(h => h.Logs.Count(l => l.CompletedDate.Date == d));
                }).ToList();

            var heatmap = Enumerable.Range(0, 30)
                .Select(i =>
                {
                    var d = today.AddDays(-29 + i);
                    return habits.Sum(h => h.Logs.Count(l => l.CompletedDate.Date == d));
                }).ToList();

            int possible = habits.Count * 30;
            int actual = habits.Sum(h => h.Logs.Count(l => l.CompletedDate >= today.AddDays(-29)));
            double rate = possible > 0 ? Math.Round((double)actual / possible * 100, 1) : 0;


            return new StatisticsViewModel
            {
                TotalHabits = habits.Count,
                CompletedToday = habits.Count(h => h.CompletedToday),
                TotalCompletions = habits.Sum(h => h.TotalCompletions),
                BestStreak = habits.Any() ? habits.Max(h => h.LongestStreak) : 0,
                CurrentStreak = habits.Any() ? habits.Max(h => h.CurrentStreak) : 0,
                CompletionRate = rate,
                Last7DaysCounts = last7Counts,
                Last7DaysLabels = last7Labels,
                HeatmapData = heatmap,
                HabitStats = habits.Select(h => new HabitStat
                {
                    Name = h.Name,
                    TotalCompletions = h.TotalCompletions,
                    CurrentStreak = h.CurrentStreak,
                    CompletionPct = possible > 0
                        ? (int)Math.Round((double)h.Logs
                            .Count(l => l.CompletedDate >= today.AddDays(-29)) / 30 * 100) : 0
                }).ToList()
            };
        }


        private void EnrichHabit(Habit habit)
        {
            var today = DateTime.UtcNow.Date;
            var dates = habit.Logs
                .Select(l => l.CompletedDate.Date)
                .OrderByDescending(d => d)
                .ToList();

            habit.CompletedToday = dates.Contains(today);
            habit.TotalCompletions = dates.Count;
            habit.CurrentStreak = CalculateCurrentStreak(dates);
            habit.LongestStreak = CalculateLongestStreak(dates);
        }


        private int CalculateCurrentStreak(List<DateTime> sortedDates)
        {
            if (!sortedDates.Any()) return 0;
            var check = DateTime.UtcNow.Date;
            if (!sortedDates.Contains(check))
            {
                check = check.AddDays(-1);
                if (!sortedDates.Contains(check)) return 0;
            }
            int streak = 0;
            while (sortedDates.Contains(check)) { streak++; check = check.AddDays(-1); }
            return streak;
        }

        private int CalculateLongestStreak(List<DateTime> sortedDates)
        {
            if (!sortedDates.Any()) return 0;
            var dates = sortedDates.OrderBy(d => d).ToList();
            int longest = 1, current = 1;
            for (int i = 1; i < dates.Count; i++)
            {
                if ((dates[i] - dates[i - 1]).TotalDays == 1) { current++; longest = Math.Max(longest, current); }
                else current = 1;
            }
            return longest;
        }
    }
}
