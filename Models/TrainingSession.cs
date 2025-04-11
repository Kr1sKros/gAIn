namespace gain.Models;

public class TrainingSession
{
    public int Id { get; set; }
    
    public DateTime SessionStartTime { get; set; }
    
    public int Duration { get; set; }

    public string FkUserId { get; set; } = string.Empty;
    
    public List<int> SessionExercises { get; set; } = new ();
}