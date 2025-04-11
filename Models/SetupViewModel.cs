using System.ComponentModel.DataAnnotations;
namespace gain.Models;

public class SetupViewModel : EquipmentCheckboxPartialModel
{
    [Required(ErrorMessage = "Please select a date of birth")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth{ get; set; }

    [Required(ErrorMessage = "Please select a fitness level")]
    public uint FitnessLevel{ get; set; }
}