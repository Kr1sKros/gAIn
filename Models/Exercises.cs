using System.ComponentModel.DataAnnotations;

namespace gain.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        
        [MaxLength(50)]
        public string Name { get; set; }
        
        public string? VideoAdress { get; set; }
        
        public string Description { get; set; }
        
        public int Difficulty { get; set; }
        
        public int? Equipment { get; set; }
        
        public string Type { get; set; }
        
        public string Primary_Musclegroup { get; set; }
        
        public string? Secondary_Musclegroup { get; set; }

    }
}