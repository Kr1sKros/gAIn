using gain.Models;
using gain.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace gain.Controllers;

public class RewardController : Controller{
    
    private readonly ApplicationDbContext _db;
    private UserManager<ApplicationUser> _um;
    


    public RewardController(ApplicationDbContext db, UserManager<ApplicationUser> um)
    {
        _db = db;
        _um = um;
    }
    
    [HttpGet]
    [Authorize]
    public IActionResult Achievements(){
        if (ModelState.IsValid)
        {
            var user = _um.GetUserAsync(User).Result;
            
            if (user == null) {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            //==================Authorize==================
            
            ViewBag.User = user;
            ViewBag.Achievementlevels = user.AchievementLevels;
        }
        var achievements = _db.Achievements.ToList();
        
        return View(achievements);
    }
}