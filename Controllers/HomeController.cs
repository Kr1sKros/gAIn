using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using gain.Models;
using gain.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace gain.Controllers;

public class HomeController : Controller
{
    
    private readonly ApplicationDbContext _db;
    private UserManager<ApplicationUser> _um;
    
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, UserManager<ApplicationUser> um)
    {
        _logger = logger;
        _db = db;
        _um = um;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Privacy()
    {
        return View();
    }
    
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Terms()
    {
        return View();
    }
    
    [HttpGet]
    [Authorize]
    public IActionResult Home()
    {
            var user = _um.GetUserAsync(User).Result;
            if (user == null){
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            //==================Authorize==================
            
            DateTime date = DateTime.Today.AddDays(-1);
            bool streakBroken = true;
            foreach (var i in _db.TrainingSessions.ToList())
            {
                if (user.Id == i.FkUserId)
                {
                    if (DateTime.Compare(i.SessionStartTime, date) > 0)
                    {
                        streakBroken = false;
                    }
                }
            }

            if (streakBroken)
            {
                user.DailyStreak = 0;
                _um.UpdateAsync(user);
            }
            
            ViewBag.Nickname = user!.Nickname;
            ViewBag.WeeklyExercise = user!.WeeklyExercise;
            ViewBag.DailyStreak = user!.DailyStreak;
            ViewBag.TotalAmountExercise = user!.TotalAmountExercise;
        
        return View();
    }


    [HttpGet]
    [Authorize]
    public IActionResult Setup()
    {
        var user = _um.GetUserAsync(User).Result;
        if (user == null) {
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }
        if (user.SetupCompleted) {
            // Setup is not configured to handle updates, send home if setup done
            return RedirectToAction(nameof(Home));
        }
        //==================Authorize==================

        // Get all equipments in db, used to generate checkbox list
        var model = new SetupViewModel();
        model.Equipments = _db.Equipments.ToList();

        return View(model);
    }

    [HttpPost]
    [Authorize]
    public IActionResult Setup(SetupViewModel model)
    {
        var user = _um.GetUserAsync(User).Result;
        
        if (user == null) {
            return View("/Areas/Identity/Pages/Account/Login.cshtml"); 
        }
        if (user.SetupCompleted) {
            // Setup is not configured to handle updates, send home if setup done
            return RedirectToAction(nameof(Home));
        }
        //==================Authorize==================
        
        user.DateOfBirth = model.DateOfBirth;
        user.FitnessLevel = model.FitnessLevel;
        user.SetupCompleted = true;


        foreach (var id in model.EquipmentId){
            user.AvailableEquipment.Add(id);
        }
        
        _um.UpdateAsync(user);
        _db.SaveChanges();
    
    
        return RedirectToAction(nameof(Home));
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Admin()
    {
        var user = _um.GetUserAsync(User).Result;
        if (user == null || !User.IsInRole("Admin"))
        {
            return RedirectToAction(nameof(Home));  
        }
        //==================Authorize==================
        
        var list = new List<ApplicationUser>();
        foreach (var usr in _um.Users) {
            if (user.Id != usr.Id) {
                list.Add(usr);
            }
        }
        
        return View(list);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult Admin(ApplicationUser updatedUser, string? uId, string action)
    {
        var user = _um.GetUserAsync(User).Result;
        if (user == null || !User.IsInRole("Admin"))
        {
            return RedirectToAction(nameof(Home));  
        }
        //==================Authorize==================


        var inspected = _um.FindByIdAsync(uId).Result;
        
        if (inspected != null && action == "view")
        {
            return RedirectToAction("AdminInspect", new {id = uId});
        }

        if (inspected != null && action == "delete")
        {
            foreach (var TS in _db.TrainingSessions) 
            { // Removes all Sessions the user had
                if (TS.FkUserId == inspected.Id) {
                    _db.TrainingSessions.Remove(TS);
                }
            }
            _um.DeleteAsync(inspected).Wait();
            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", inspected.Id);
            return RedirectToAction(nameof(Admin));
        }
        
        
        var oldUser = _um.FindByIdAsync(updatedUser.Id).Result;
        if (oldUser == null)
        {
            return RedirectToAction(nameof(Admin));
        }
        oldUser.Email = updatedUser.Email;
        oldUser.EmailConfirmed = updatedUser.EmailConfirmed;
        oldUser.TwoFactorEnabled = updatedUser.TwoFactorEnabled;
        oldUser.Nickname = updatedUser.Nickname;
        oldUser.DateOfBirth = updatedUser.DateOfBirth;
        oldUser.FitnessLevel = updatedUser.FitnessLevel;
        oldUser.SetupCompleted = updatedUser.SetupCompleted;
        oldUser.TotalAmountExercise = updatedUser.TotalAmountExercise;
        oldUser.WeeklyExercise = updatedUser.WeeklyExercise;
        oldUser.DailyStreak = updatedUser.DailyStreak;

        _um.UpdateAsync(oldUser);
        
        return RedirectToAction(nameof(Admin));
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult AdminInspect(string id)
    {
        var user = _um.GetUserAsync(User).Result;
        if (user == null || !User.IsInRole("Admin"))
        {
            return RedirectToAction(nameof(Home));  
        }
        //==================Authorize==================
        
        var inspected = _um.FindByIdAsync(id).Result;
        var model = new AdminInspectViewModel();
        
        model.Equipments = _db.Equipments.ToList();
        model.EquipmentId = inspected.AvailableEquipment;
        foreach (var i in _db.TrainingSessions)
        {
            if (i.FkUserId == inspected.Id)
            {
                model.Sessions.Add(i);
            }
        }
        
        return View(model);
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult AdminInspect(string id, EquipmentCheckboxPartialModel updated)
    {
        var user = _um.GetUserAsync(User).Result;
        if (user == null || !User.IsInRole("Admin"))
        {
            return RedirectToAction(nameof(Home));  
        }
        //==================Authorize==================
        
        var inspected = _um.FindByIdAsync(id).Result;
        inspected.AvailableEquipment = new List<int>();

        _um.UpdateAsync(inspected);
        
        foreach (var newId in updated.EquipmentId)
        {
            inspected.AvailableEquipment.Add(newId);
        }
        
        _um.UpdateAsync(inspected);
        
        return RedirectToAction(nameof(Admin));
    }
    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
