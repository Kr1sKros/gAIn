using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace gain.Models;

public class ApplicationUser : IdentityUser{
    
    [PersonalData]
    [MaxLength(30)]
    public string Nickname{ get; set; } = string.Empty;

    [PersonalData]
    [Required(ErrorMessage = "Please select a date of birth")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth{ get; set; }

    [PersonalData]
    [Required(ErrorMessage = "Please select a fitness level")]
    [Range(0, 5, ErrorMessage = "Between 0 and 5")]
    public uint FitnessLevel{ get; set; } // 0 = not set

    [PersonalData]
    public bool SetupCompleted{ get; set; } = false;
    
    [PersonalData]
    public uint TotalAmountExercise{ get; set; } = 0;
    
    // [PersonalData] Automatic parsing of lists not possible
    public List<int> AchievementLevels { get; set; } = new List<int>(){0, 0, 0};
    
    [PersonalData]
    public uint WeeklyExercise{ get; set; } = 0;
    
    [PersonalData]
    public uint DailyStreak{ get; set; } = 0;
    
    [PersonalData]
    public int AdjustmentLevel{ get; set; } = 0;

    public List<int> AvailableEquipment { get; set; } = new();
}