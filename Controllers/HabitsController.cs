using HabitTracker.Models;
using HabitTracker.Models.ViewModels;
using HabitTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.Controllers
{
    [Authorize]
    public class HabitsController : Controller
    {
        private readonly HabitService _habitService;
        private readonly UserManager<ApplicationUser> _userManager;

        public HabitsController(HabitService habitService, UserManager<ApplicationUser> userManager)
        {
            _habitService = habitService;
            _userManager = userManager;
        }

        private string UserId => _userManager.GetUserId(User)!;

        public async Task<IActionResult> Index()
        {
            var habits = await _habitService.GetUserHabitsAsync(UserId);
            return View(habits);
        }

        [HttpGet]
        public IActionResult Create() => View(new HabitFormViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HabitFormViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _habitService.CreateHabitAsync(new Habit
            {
                Name = model.Name,
                Description = model.Description,
                Frequency = model.Frequency,
                UserId = UserId
            });

            TempData["Success"] = $"Habit \"{model.Name}\" created successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var habit = await _habitService.GetHabitByIdAsync(id, UserId);
            if (habit == null) return NotFound();

            return View(new HabitFormViewModel
            {
                Id = habit.Id,
                Name = habit.Name,
                Description = habit.Description,
                Frequency = habit.Frequency
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HabitFormViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var success = await _habitService.UpdateHabitAsync(new Habit
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Frequency = model.Frequency
            }, UserId);

            if (!success) return NotFound();

            TempData["Success"] = "Habit updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var habit = await _habitService.GetHabitByIdAsync(id, UserId);
            if (habit == null) return NotFound();
            return View(habit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _habitService.DeleteHabitAsync(id, UserId);
            TempData["Success"] = "Habit deleted.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleComplete(int id)
        {
            await _habitService.ToggleCompletionAsync(id, UserId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleCompleteFromDashboard(int id)
        {
            await _habitService.ToggleCompletionAsync(id, UserId);
            return RedirectToAction("Index", "Home");
        }
    }
}
