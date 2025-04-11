namespace gain.Models;

public class EquipmentCheckboxPartialModel
{
    // For displaying equipment
    public IEnumerable<Equipment> Equipments{ get; set; } = Enumerable.Empty<Equipment>();
    
    // Stores id of selected equipment
    public List<int> EquipmentId{ get; set; } = new ();
    
}