using Microsoft.AspNetCore.Mvc;
using gain.Models;
using gain.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Newtonsoft.Json;
using OpenAITest.Services;

namespace gain.Controllers;

public class WorkoutController : Controller
{
    private readonly ApplicationDbContext _db;
    private UserManager<ApplicationUser> _um;
    
    private readonly OpenAIService _openAIService;

    public WorkoutController(ApplicationDbContext db, UserManager<ApplicationUser> um, OpenAIService openAiService)
    {
        _db = db;
        _um = um;
        _openAIService = openAiService;
    }

    [HttpGet]
    [Authorize]
    public IActionResult Setupworkout()
    {
        var user = _um.GetUserAsync(User).Result;
        if (user == null){
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }
        if (!user.SetupCompleted){
            return RedirectToAction("Setup", "Home");
        }
        //==================Authorize==================
        
        /*
        var setup = new SetupWorkoutViewModel();
        
        if (ModelState.IsValid){
            setup.Equipments = _db.Equipments.ToList();
            foreach (var eq in user.AvailableEquipment)
            {
                setup.EquipmentId.Add(eq);
            }
        }
        */

        var setup = new EquipmentCheckboxPartialModel();

        if (ModelState.IsValid)
        {
            setup.Equipments = _db.Equipments.ToList();
            foreach (var eq in user.AvailableEquipment)
            {
                setup.EquipmentId.Add(eq);
            }
        }
        
        return View(setup);
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> GenerateWorkout(int duration, List<int> equipmentId)
    {
        var user = _um.GetUserAsync(User).Result;
        if (user == null){
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }
        if (!user.SetupCompleted){
            return RedirectToAction("Setup", "Home");
        }
        //==================Authorize==================
        
        uint userLevel;
        userLevel = user.FitnessLevel;
        Console.WriteLine("This is the users fitness level " + userLevel);
        if (equipmentId == null) return RedirectToAction("Setup", "Home");
        
        
        var response = await _openAIService.GenerateWorkoutAsync(duration, userLevel, equipmentId);
        List<int> exerciseIds = new List<int>();
        try
        {
            exerciseIds = response.Split(',')
                .Select(id => int.Parse(id.Trim()))
                .ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            string errorMessage = "Something went wrong with the response from the AI... Please try again later.";
            return RedirectToAction("FailedGeneration");
        }
        
        return RedirectToAction(nameof(Workout), new { duration, exerciseIds });
    }
    
    [HttpGet]
    [Authorize]
    public IActionResult Workout(int duration, List<int> exerciseIds)
    {
        var user = _um.GetUserAsync(User).Result;
        if (user == null){
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }
        if (!user.SetupCompleted){
            return RedirectToAction("Setup", "Home");
        }
        //==================Authorize==================
        
        
        var exercises = _db.Exercises
            .Where(e => exerciseIds.Contains(e.Id))
            .ToList();

        ViewBag.Duration = duration;
        
        return View(exercises);
    }

    [HttpGet]
    public static void AddSession(ApplicationDbContext db, string userId, int duration, List<int> exerciseIds)
    {
        var session = new TrainingSession()
        {
            SessionStartTime = DateTime.Now,
            Duration = duration,
            FkUserId = userId,
            SessionExercises = exerciseIds
        };
        
        db.TrainingSessions.Add(session);
        db.SaveChanges();
    }
    
    [HttpPost]
    public async Task<IActionResult> LogSession(int duration, string exerciseData)
    {
        
        if (exerciseData == null || exerciseData == "")
        {
            return BadRequest("No exercises found :(");
        }
        
        List<Exercise> exercises = JsonConvert.DeserializeObject<List<Exercise>>(exerciseData);
        
        // Logs the exercise to the database
        List<int> exerciseIds = exercises.Select(e => e.Id).ToList();
        
        var user = _um.GetUserAsync(User).Result;
        string userId = user.Id;
        
        
        // Updates the weekly amount of exercise (and in turn the total amount)
        user.WeeklyExercise += (uint)duration;
        user.TotalAmountExercise += (uint)duration;
        
        _um.UpdateAsync(user);
        
        // If it's the first session of today, update the users achievement score
        DateTime date = DateTime.Today;
        bool allreadyTrainedToday = false;
        foreach (var i in _db.TrainingSessions)
        {
            if (i.FkUserId == userId)
            {
                if (DateTime.Compare(i.SessionStartTime, date) > 0)
                {
                    allreadyTrainedToday = true;
                }
            }
        }
        if (allreadyTrainedToday == false)
        {
            user.DailyStreak += 1;
            _um.UpdateAsync(user);
        }
        
        // Check if the user has achieved enough to update their level
            List<int> achievementLevels = user.AchievementLevels.ToList();
            List<uint> userFields = new List<uint>()
                {user.DailyStreak, user.FitnessLevel, user.TotalAmountExercise};
            bool changeHappened = false;
            int n = 0;
            List<int> changedLevels = new List<int>();
            
            //Checks all achivements for level up
            foreach (var i in _db.Achievements.ToList())
            {
                uint nextLevelRequirement = i.Requirements[achievementLevels[n]+1];
                if (userFields[n] >= nextLevelRequirement)
                {
                    changedLevels.Add(achievementLevels[n]+1);
                    changeHappened = true;
                }
                else
                {
                    changedLevels.Add(achievementLevels[n]);
                }
                n++;
            }
            if (changeHappened)
            {
                user.AchievementLevels = changedLevels;
                _um.UpdateAsync(user);
            }

        
        AddSession(_db, userId, duration, exerciseIds);

        return RedirectToAction("Feedback", "Workout");
    }

    [HttpPost]
    public async Task<IActionResult> AdjustSkillLevel(int adjustment)
    {
        var user = _um.GetUserAsync(User).Result;
        
        user.AdjustmentLevel += adjustment;
        
        Console.WriteLine(adjustment);
        Console.WriteLine(user.AdjustmentLevel);

        if (Math.Abs(user.AdjustmentLevel) >= 3)
        {
            if (user.FitnessLevel != 1 && user.FitnessLevel != 5)
            {
                int newLevel = (int)user.FitnessLevel + Math.Sign(user.AdjustmentLevel);
                
                user.FitnessLevel = (uint)newLevel;
                
            }
            user.AdjustmentLevel = 0;
        }
        
        _um.UpdateAsync(user);
        
        return RedirectToAction("Home", "Home");
    }

    [Authorize]
    public IActionResult Feedback()
    {
        return View();
    }
    
    public IActionResult FailedGeneration()
    {
        return View();
    }
}