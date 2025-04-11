namespace gain.Models;

public class AdminInspectViewModel : EquipmentCheckboxPartialModel
{
    public uint Id { get; set; }
    public List<TrainingSession> Sessions { get; set; } = new List<TrainingSession>();
}