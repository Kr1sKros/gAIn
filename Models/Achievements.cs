namespace gain.Models;

public class Achievements
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public List<uint> Requirements { get; set; } = new ();
    
    public string Description { get; set; } = string.Empty;
    
    
    
}