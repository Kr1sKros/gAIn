namespace gain.Models;

public class Equipment{
    
    public int Id{ get; set; }
    
    public string Name{ get; set; } = String.Empty;
    
    public string ImagePath{ get; set; } = String.Empty;
    
    public Equipment(){}

    public Equipment(string name, string imagePath){
        Name = name;
        ImagePath = imagePath;
    }
}