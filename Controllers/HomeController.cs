using HabitTracker.Models;
using HabitTracker.Models.ViewModels;
using HabitTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly HabitService _habitService;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(HabitService habitService, UserManager<ApplicationUser> userManager)
        {
            _habitService = habitService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity!.IsAuthenticated)
                return View("Landing");

            var userId = _userManager.GetUserId(User)!;
            var user = await _userManager.GetUserAsync(User);
            var habits = await _habitService.GetUserHabitsAsync(userId);

            var vm = new DashboardViewModel
            {
                Habits = habits,
                TotalHabits = habits.Count,
                CompletedToday = habits.Count(h => h.CompletedToday),
                TotalCompletionsAllTime = habits.Sum(h => h.TotalCompletions),
                BestStreak = habits.Any() ? habits.Max(h => h.LongestStreak) : 0,
                CurrentStreak = habits.Any() ? habits.Max(h => h.CurrentStreak) : 0,
                UserName = user?.FullName ?? user?.Email ?? "there"
            };

            return View("Dashboard", vm);
        }

        [Authorize]
        public async Task<IActionResult> Statistics()
        {
            var userId = _userManager.GetUserId(User)!;
            var vm = await _habitService.GetStatisticsAsync(userId);
            return View(vm);
        }
    }
}
