using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using gain.Models;

namespace gain.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options){
    }
    
    // List of all equipments stored
    public DbSet<Equipment> Equipments => Set<Equipment>();

    // List of all exercises stored
    public DbSet<Exercise> Exercises => Set<Exercise>();
    
    // Stores one complete exercise. Also stores id of user
    public DbSet<TrainingSession> TrainingSessions => Set<TrainingSession>();
    
    //List of all achivements
    public DbSet<Achievements> Achievements => Set<Achievements>();

}
