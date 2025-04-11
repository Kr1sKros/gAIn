using gain.Models;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace gain.Data;

public class ApplicationDbInitializer
{
    public static void Initialize(ApplicationDbContext db, UserManager<ApplicationUser> um, RoleManager<IdentityRole> rm)
    {
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        AddEquipments(db);
        AddExercise(db);
        AddAchievements(db);

        // Create roles
        if (!rm.RoleExistsAsync("Admin").Result)
        {
            var adminRole = new IdentityRole("Admin");
            rm.CreateAsync(adminRole).Wait();
        }
        // Create user
        var adminEmail = "admin@gain.no";
        if (um.FindByEmailAsync(adminEmail).Result == null)
        {
            var admin = new ApplicationUser
            {
                Nickname = "John Gain",
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            um.CreateAsync(admin, "Password1.").Wait();
            um.AddToRoleAsync(admin, "Admin").Wait();
        }
        
        AddUsers(db, um);

        db.SaveChanges();
    }


    private static void AddEquipments(ApplicationDbContext db)
    {
        // Add equipment
        if (!db.Equipments.Any())
        {
            var equipments = new[]
            {
                new Equipment("Dumbbell", "/images/dumbbells.png"),
                new Equipment("Pull-up bar", "/images/pull-up-bar.png"),
                new Equipment("Resistance band", "/images/resistance-band.png"),
                new Equipment("Medicine ball", "/images/medicine-ball.png"),
            };
            db.Equipments.AddRange(equipments);
            db.SaveChanges();
        }
    }

    private static void AddSession(ApplicationDbContext db, UserManager<ApplicationUser> um, string email)
    {
        var user = um.FindByEmailAsync(email).Result;
        var session1 = new TrainingSession()
        {
            SessionStartTime = DateTime.Now,
            Duration = 10000,
            FkUserId = user.Id,
            SessionExercises = new List<int>(){1,2,3}
        };
        var session2 = new TrainingSession()
        {
            SessionStartTime = DateTime.Now,
            Duration = 20000,
            FkUserId = user.Id,
            SessionExercises = new List<int>(){2,3,4}
        };
        var session3 = new TrainingSession()
        {
            SessionStartTime = DateTime.Now,
            Duration = 30000,
            FkUserId = user.Id,
            SessionExercises = new List<int>(){3,4,5}
        };
        db.TrainingSessions.Add(session1);
        db.TrainingSessions.Add(session2);
        db.TrainingSessions.Add(session3);
    }

    private static void AddUsers(ApplicationDbContext db, UserManager<ApplicationUser> um)
    {
        var user1 = new ApplicationUser
        {
            Nickname = "user1",
            UserName = "user1@mail.com",
            Email = "user1@mail.com",
            EmailConfirmed = true,
            DateOfBirth = DateTime.Today,
            FitnessLevel = 0, // Default
            SetupCompleted = false,
            TotalAmountExercise = 600,
            WeeklyExercise = 60,
            DailyStreak = 1001,
            AchievementLevels = new List<int>(){13, 0, 3}
        };
        um.CreateAsync(user1, "Password1.").Wait();
        AddSession(db, um, "user1@mail.com");
        
        var user2 = new ApplicationUser
        {
            Nickname = "user2",
            UserName = "user2@mail.com",
            Email = "user2@mail.com",
            EmailConfirmed = true,
            DateOfBirth = DateTime.Today,
            FitnessLevel = 2,
            SetupCompleted = true,
            TotalAmountExercise = 600,
            WeeklyExercise = 60,
            DailyStreak = 1001,
            AchievementLevels = new List<int>(){13, 2, 3}
        };
        um.CreateAsync(user2, "Password2.").Wait();
        
        var user3 = new ApplicationUser
        {
            Nickname = "user3",
            UserName = "user3@mail.com",
            Email = "user3@mail.com",
            EmailConfirmed = true,
            DateOfBirth = DateTime.Today,
            FitnessLevel = 1,
            SetupCompleted = false,
            TotalAmountExercise = 300,
            WeeklyExercise = 30,
            DailyStreak = 500,
            AchievementLevels = new List<int>(){13, 1, 2}
        };
        um.CreateAsync(user3, "Password3.").Wait();

        var user4 = new ApplicationUser
        {
            Nickname = "user4",
            UserName = "user4@mail.com",
            Email = "user4@mail.com",
            EmailConfirmed = true,
            DateOfBirth = DateTime.Today.AddYears(-1),
            FitnessLevel = 3,
            SetupCompleted = true,
            TotalAmountExercise = 900,
            WeeklyExercise = 90,
            DailyStreak = 1200,
            AchievementLevels = new List<int>(){14, 3, 3}
        };
        um.CreateAsync(user4, "Password4.").Wait();

        var user5 = new ApplicationUser
        {
            Nickname = "user5",
            UserName = "user5@mail.com",
            Email = "user5@mail.com",
            EmailConfirmed = false,
            DateOfBirth = DateTime.Today.AddYears(-5),
            FitnessLevel = 4,
            SetupCompleted = false,
            TotalAmountExercise = 1500,
            WeeklyExercise = 150,
            DailyStreak = 2500,
            AchievementLevels = new List<int>(){15, 4, 4}
        };
        um.CreateAsync(user5, "Password5.").Wait();

        var user6 = new ApplicationUser
        {
            Nickname = "user6",
            UserName = "user6@mail.com",
            Email = "user6@mail.com",
            EmailConfirmed = true,
            DateOfBirth = DateTime.Today.AddYears(-10),
            FitnessLevel = 0,
            SetupCompleted = false,
            TotalAmountExercise = 100,
            WeeklyExercise = 10,
            DailyStreak = 100,
            AchievementLevels = new List<int>(){12, 0, 2}
        };
        um.CreateAsync(user6, "Password6.").Wait();

        var user7 = new ApplicationUser
        {
            Nickname = "user7",
            UserName = "user7@mail.com",
            Email = "user7@mail.com",
            EmailConfirmed = true,
            DateOfBirth = DateTime.Today,
            FitnessLevel = 2,
            SetupCompleted = true,
            TotalAmountExercise = 600,
            WeeklyExercise = 60,
            DailyStreak = 1100,
            AchievementLevels = new List<int>(){14, 2, 3}
        };
        um.CreateAsync(user7, "Password7.").Wait();

        var user8 = new ApplicationUser
        {
            Nickname = "user8",
            UserName = "user8@mail.com",
            Email = "user8@mail.com",
            EmailConfirmed = true,
            DateOfBirth = DateTime.Today,
            FitnessLevel = 1,
            SetupCompleted = false,
            TotalAmountExercise = 400,
            WeeklyExercise = 40,
            DailyStreak = 700,
            AchievementLevels = new List<int>(){13, 1, 3}
        };
        um.CreateAsync(user8, "Password8.").Wait();

        var user9 = new ApplicationUser
        {
            Nickname = "user9",
            UserName = "user9@mail.com",
            Email = "user9@mail.com",
            EmailConfirmed = true,
            DateOfBirth = DateTime.Today,
            FitnessLevel = 5,
            SetupCompleted = true,
            TotalAmountExercise = 2000,
            WeeklyExercise = 200,
            DailyStreak = 3000,
            AchievementLevels = new List<int>(){15, 5, 4}
        };
        um.CreateAsync(user9, "Password9.").Wait();
        
        
        db.SaveChanges();
    }

    private static void AddAchievements(ApplicationDbContext db)
    {
        if (!db.Achievements.Any())
        {
            var achievements = new[]
            {
                new Achievements
                {
                    Id = 1,
                    Name = "Streak",
                    Description = "Maintain a streak for a certain amount of time",
                    Requirements = [0, 1, 2, 3, 4, 5, 6, 7, 14, 28, 42, 56, 70, 105, 1000, 1500]
                },
                new Achievements
                {
                    Id = 2,
                    Name = "Fitness level",
                    Description = "Maintain a fitness level",
                    Requirements = [0, 1, 2, 3, 4, 5]
                },
                new Achievements
                {
                    Id = 3,
                    Name = "Minutes worked out",
                    Description = "Train for a total amount of minutes",
                    Requirements = [0, 10, 69, 420, 1000, 5000, 10000, 20000]
                }
            };
            db.Achievements.AddRange(achievements);
            db.SaveChanges();
        }
    }

    private static void AddExercise(ApplicationDbContext db)
    {
        // Add exercises
        if (!db.Exercises.Any())
        {
            /*
            var exercises = new[]
            {
                new Exercise
                {
                    Id = 1,
                    Name = "Wall Pushups",
                    VideoAdress = null,
                    Description = "Push against the wall at an incline, engaging chest and arms.",
                    Difficulty = 1,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Chest",
                    Secondary_Musclegroup = "Triceps"
                },
                new Exercise
                {
                    Id = 2,
                    Name = "Seated Forward Bend",
                    VideoAdress = null,
                    Description = "Sit with legs extended, reach for your toes to stretch hamstrings.",
                    Difficulty = 1,
                    Equipment = null,
                    Type = "Stretching",
                    Primary_Musclegroup = "Hamstrings",
                    Secondary_Musclegroup = "Lower Back"
                },
                new Exercise
                {
                    Id = 3,
                    Name = "Knee Pushups",
                    VideoAdress = null,
                    Description = "Perform a pushup on knees for modified strength training.",
                    Difficulty = 3,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Chest",
                    Secondary_Musclegroup = "Triceps"
                },
                new Exercise
                {
                    Id = 4,
                    Name = "Incline Pushups",
                    VideoAdress = null,
                    Description = "Place hands on a raised surface (e.g., bench) to perform pushups.",
                    Difficulty = 2,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Chest",
                    Secondary_Musclegroup = "Triceps"
                },
                new Exercise
                {
                    Id = 5,
                    Name = "Glute Bridge",
                    VideoAdress = null,
                    Description = "Lay on your back, lift hips to engage glutes and hamstrings.",
                    Difficulty = 1,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Glutes",
                    Secondary_Musclegroup = "Hamstrings"
                },
                new Exercise
                {
                    Id = 6,
                    Name = "Plank on Knees",
                    VideoAdress = null,
                    Description = "Hold your body straight supported on knees and elbows.",
                    Difficulty = 2,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Core",
                    Secondary_Musclegroup = "Lower Back"
                },
                new Exercise
                {
                    Id = 7,
                    Name = "Side Plank",
                    VideoAdress = null,
                    Description = "Support yourself on one elbow, stacking feet and holding body straight.",
                    Difficulty = 3,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Core",
                    Secondary_Musclegroup = "Obliques"
                },
                new Exercise
                {
                    Id = 8,
                    Name = "Standing Calf Raises",
                    VideoAdress = null,
                    Description = "Stand tall and lift heels off the ground to strengthen calves.",
                    Difficulty = 1,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Calves",
                    Secondary_Musclegroup = "Ankles"
                },
                new Exercise
                {
                    Id = 9,
                    Name = "Chair Dips",
                    VideoAdress = null,
                    Description = "Using a chair, lower and lift your body to work arms and shoulders.",
                    Difficulty = 3,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Triceps",
                    Secondary_Musclegroup = "Shoulders"
                },
                new Exercise
                {
                    Id = 10,
                    Name = "Leg Lifts",
                    VideoAdress = null,
                    Description = "Lie on your back and lift legs up while keeping them straight.",
                    Difficulty = 2,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Core",
                    Secondary_Musclegroup = "Lower Back"
                },
                new Exercise
                {
                    Id = 11,
                    Name = "Hip Flexor Stretch",
                    VideoAdress = null,
                    Description = "Lunge forward and hold to stretch hip flexors.",
                    Difficulty = 1,
                    Equipment = null,
                    Type = "Stretching",
                    Primary_Musclegroup = "Hip Flexors",
                    Secondary_Musclegroup = "Quads"
                },
                new Exercise
                {
                    Id = 12,
                    Name = "Hamstring Stretch",
                    VideoAdress = null,
                    Description = "While standing, bend at the waist and reach for your toes.",
                    Difficulty = 1,
                    Equipment = null,
                    Type = "Stretching",
                    Primary_Musclegroup = "Hamstrings",
                    Secondary_Musclegroup = "Lower Back"
                },
                new Exercise
                {
                    Id = 13,
                    Name = "High Knees",
                    VideoAdress = null,
                    Description = "Run in place while lifting knees high.",
                    Difficulty = 4,
                    Equipment = null,
                    Type = "Cardio",
                    Primary_Musclegroup = "Quads",
                    Secondary_Musclegroup = "Calves"
                },
                new Exercise
                {
                    Id = 14,
                    Name = "Jump Squats",
                    VideoAdress = null,
                    Description = "Perform a squat and jump up explosively.",
                    Difficulty = 5,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Quads",
                    Secondary_Musclegroup = "Glutes"
                },
                new Exercise
                {
                    Id = 15,
                    Name = "Plank Shoulder Taps",
                    VideoAdress = null,
                    Description = "From a plank, touch alternate shoulders with opposite hands.",
                    Difficulty = 4,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Core",
                    Secondary_Musclegroup = "Shoulders"
                },
                new Exercise
                {
                    Id = 16,
                    Name = "Modified Burpees",
                    VideoAdress = null,
                    Description = "From standing, step back into a plank and return to standing.",
                    Difficulty = 4,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Full Body",
                    Secondary_Musclegroup = "Cardio"
                },
                new Exercise
                {
                    Id = 17,
                    Name = "Tuck Jumps",
                    VideoAdress = null,
                    Description = "Jump high, tucking knees toward your chest.",
                    Difficulty = 5,
                    Equipment = null,
                    Type = "Cardio",
                    Primary_Musclegroup = "Quads",
                    Secondary_Musclegroup = "Calves"
                },
                new Exercise
                {
                    Id = 18,
                    Name = "Mountain Climbers",
                    VideoAdress = null,
                    Description = "In plank position, alternate driving knees toward your chest.",
                    Difficulty = 5,
                    Equipment = null,
                    Type = "Cardio",
                    Primary_Musclegroup = "Core",
                    Secondary_Musclegroup = "Quads"
                },
                new Exercise
                {
                    Id = 19,
                    Name = "Lunge with Twist",
                    VideoAdress = null,
                    Description = "Lunge forward, twisting torso toward the leading leg.",
                    Difficulty = 3,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Quads",
                    Secondary_Musclegroup = "Core"
                },
                new Exercise
                {
                    Id = 20,
                    Name = "Step-Ups",
                    VideoAdress = null,
                    Description = "Step onto a raised platform, alternating legs.",
                    Difficulty = 3,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Quads",
                    Secondary_Musclegroup = "Glutes"
                }
            };
            */

            var exercises = new[]
            {
                new Exercise
                {
                    Name = "Abdominal Stretch",
                    VideoAdress = "/images/workoutGifs/abdominal-stretch.gif",
                    Description = "Stretch your abdominals by extending your torso while keeping your hips grounded.",
                    Difficulty = 1,
                    Equipment = null,
                    Type = "Stretching",
                    Primary_Musclegroup = "Abdominals",
                    Secondary_Musclegroup = "Lower Back"
                },

                new Exercise
                {
                    Name = "Above Head Chest Stretch",
                    VideoAdress = "/images/workoutGifs/Above-Head-Chest-Stretch.gif",
                    Description = "Raise your arms overhead and stretch your chest and shoulders.",
                    Difficulty = 1,
                    Equipment = null,
                    Type = "Stretching",
                    Primary_Musclegroup = "Chest",
                    Secondary_Musclegroup = "Shoulders"
                },

                new Exercise
                {
                    Name = "Alternate Leg Raises",
                    VideoAdress = "/images/workoutGifs/Alternate-Leg-Raises.gif",
                    Description = "Raise your legs alternately while engaging your core muscles.",
                    Difficulty = 2,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Abdominals",
                    Secondary_Musclegroup = "Hip Flexors"
                },

                new Exercise
                {
                    Name = "Alternating Dumbbell Front Raise",
                    VideoAdress = "/images/workoutGifs/Alternating-Dumbbell-Front-Raise.gif",
                    Description = "Lift dumbbells alternately in front of you to shoulder height.",
                    Difficulty = 2,
                    Equipment = 1,
                    Type = "Strength",
                    Primary_Musclegroup = "Shoulders",
                    Secondary_Musclegroup = "Chest"
                },

                new Exercise
                {
                    Name = "Archer Pull-Up",
                    VideoAdress = "/images/workoutGifs/Archer-Pull-up.gif",
                    Description = "Perform a wide-grip pull-up, alternating arms in an archer motion.",
                    Difficulty = 5,
                    Equipment = 2,
                    Type = "Strength",
                    Primary_Musclegroup = "Back",
                    Secondary_Musclegroup = "Biceps"
                },

                new Exercise
                {
                    Name = "Arm Circles",
                    VideoAdress = "/images/workoutGifs/arm-circles.gif",
                    Description = "Rotate your arms in circles to warm up the shoulder muscles.",
                    Difficulty = 1,
                    Equipment = null,
                    Type = "Stretching",
                    Primary_Musclegroup = "Shoulders",
                    Secondary_Musclegroup = "Triceps"
                },

                new Exercise
                {
                    Name = "Arm Circles Shoulders",
                    VideoAdress = "/images/workoutGifs/Arm-Circles_Shoulders.gif",
                    Description = "Rotate your shoulders in small circles to improve flexibility.",
                    Difficulty = 1,
                    Equipment = null,
                    Type = "Stretching",
                    Primary_Musclegroup = "Shoulders",
                    Secondary_Musclegroup = "Upper Back"
                },

                new Exercise
                {
                    Name = "Arm Scissors",
                    VideoAdress = "/images/workoutGifs/Arm-Scissors.gif",
                    Description = "Cross your arms in front of your chest in a scissor motion.",
                    Difficulty = 1,
                    Equipment = null,
                    Type = "Stretching",
                    Primary_Musclegroup = "Chest",
                    Secondary_Musclegroup = "Shoulders"
                },

                new Exercise
                {
                    Name = "Back Slaps Wrap Around Stretch",
                    VideoAdress = "/images/workoutGifs/Back-Slaps-Wrap-Around-Stretch.gif",
                    Description = "Swing your arms around and slap your back to stretch your shoulders.",
                    Difficulty = 1,
                    Equipment = null,
                    Type = "Stretching",
                    Primary_Musclegroup = "Shoulders",
                    Secondary_Musclegroup = "Chest"
                },

                new Exercise
                {
                    Name = "Biceps Leg Concentration Curl",
                    VideoAdress = "/images/workoutGifs/Biceps-Leg-Concentration-Curl.gif",
                    Description = "Perform a concentration curl while seated, resting your elbow on your leg.",
                    Difficulty = 2,
                    Equipment = 1,
                    Type = "Strength",
                    Primary_Musclegroup = "Biceps",
                    Secondary_Musclegroup = "Forearms"
                },
                
                new Exercise
                {
                    Name = "Bicycle Crunch",
                    VideoAdress = "/images/workoutGifs/Bicycle-Crunch.gif",
                    Description = "Engage your core by bringing your opposite elbow to your opposite knee while extending the other leg.",
                    Difficulty = 3,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Abdominals",
                    Secondary_Musclegroup = "Hip Flexors"
                },

                new Exercise
                {
                    Name = "Bodyweight Lunges",
                    VideoAdress = "/images/workoutGifs/bodyweight-lunges.gif",
                    Description = "Step forward into a lunge, lowering your hips to work your quads and glutes.",
                    Difficulty = 2,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Quadriceps",
                    Secondary_Musclegroup = "Glutes"
                },

                new Exercise
                {
                    Name = "Bodyweight Squat",
                    VideoAdress = "/images/workoutGifs/Bodyweight-Squat.gif",
                    Description = "Perform a squat using your body weight for resistance, engaging your lower body.",
                    Difficulty = 2,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Quadriceps",
                    Secondary_Musclegroup = "Glutes"
                },

                new Exercise
                {
                    Name = "Bodyweight Sumo Squat",
                    VideoAdress = "/images/workoutGifs/BODYWEIGHT-SUMO-SQUAT.gif",
                    Description = "Widen your stance and squat deeply to target your inner thighs and glutes.",
                    Difficulty = 2,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Quadriceps",
                    Secondary_Musclegroup = "Inner Thighs"
                },
                
                new Exercise
                {
                    Name = "Brachialis Pull-Up",
                    VideoAdress = "/images/workoutGifs/Brachialis-Pull-up.gif",
                    Description = "Perform a pull-up focusing on the brachialis muscles using a neutral grip.",
                    Difficulty = 4,
                    Equipment = 2,
                    Type = "Strength",
                    Primary_Musclegroup = "Biceps",
                    Secondary_Musclegroup = "Back"
                },

                new Exercise
                {
                    Name = "Burpees",
                    VideoAdress = "/images/workoutGifs/burpees.gif",
                    Description = "A full-body exercise that involves a squat, jump, and push-up for cardio and strength.",
                    Difficulty = 4,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Full Body",
                    Secondary_Musclegroup = "Legs"
                },

                new Exercise
                {
                    Name = "Chest Tap Push-Up",
                    VideoAdress = "/images/workoutGifs/Chest-Tap-Push-up.gif",
                    Description = "Perform a push-up and tap your chest with one hand between reps.",
                    Difficulty = 3,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Chest",
                    Secondary_Musclegroup = "Triceps"
                },

                new Exercise
                {
                    Name = "Chin-Up",
                    VideoAdress = "/images/workoutGifs/Chin-Up.gif",
                    Description = "Pull yourself up on a bar with a supinated (underhand) grip to engage your biceps and back.",
                    Difficulty = 4,
                    Equipment = 2,
                    Type = "Strength",
                    Primary_Musclegroup = "Biceps",
                    Secondary_Musclegroup = "Back"
                },

                new Exercise
                {
                    Name = "Clap Push-Up",
                    VideoAdress = "/images/workoutGifs/Clap-Push-Up.gif",
                    Description = "An explosive push-up where you clap your hands between reps to build power.",
                    Difficulty = 4,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Chest",
                    Secondary_Musclegroup = "Shoulders"
                },

                new Exercise
                {
                    Name = "Close Grip Dumbbell Press",
                    VideoAdress = "/images/workoutGifs/Close-Grip-Dumbbell-Press.gif",
                    Description = "Lie on a bench and press dumbbells with a close grip to emphasize the triceps.",
                    Difficulty = 3,
                    Equipment = 1,
                    Type = "Strength",
                    Primary_Musclegroup = "Chest",
                    Secondary_Musclegroup = "Triceps"
                },

                new Exercise
                {
                    Name = "Close Grip Knee Push-Up",
                    VideoAdress = "/images/workoutGifs/Close-Grip-Knee-Push-up.gif",
                    Description = "Perform a push-up from your knees with a close grip to target your triceps.",
                    Difficulty = 1,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Triceps",
                    Secondary_Musclegroup = "Chest"
                },

                new Exercise
                {
                    Name = "Cross Body Mountain Climber",
                    VideoAdress = "/images/workoutGifs/Cross-Body-Mountain-Climber.gif",
                    Description = "Bring your knees to the opposite elbows while maintaining a plank position to engage the core.",
                    Difficulty = 2,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Abdominals",
                    Secondary_Musclegroup = "Shoulders"
                },

                new Exercise
                {
                    Name = "Cross Body Push-Up Plyometric",
                    VideoAdress = "/images/workoutGifs/Cross-Body-Push-up_Plyometric.gif",
                    Description = "Perform an explosive push-up, bringing one knee toward the opposite elbow.",
                    Difficulty = 4,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Chest",
                    Secondary_Musclegroup = "Core"
                },

                new Exercise
                {
                    Name = "Cross Crunch",
                    VideoAdress = "/images/workoutGifs/Cross-Crunch.gif",
                    Description = "Lie on your back and bring your elbow to the opposite knee while crunching.",
                    Difficulty = 2,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Abdominals",
                    Secondary_Musclegroup = "Obliques"
                },

                new Exercise
                {
                    Name = "Crunch",
                    VideoAdress = "/images/workoutGifs/Crunch.gif",
                    Description = "Lift your upper back off the ground by contracting your core muscles.",
                    Difficulty = 1,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Abdominals",
                    Secondary_Musclegroup = null
                },

                new Exercise
                {
                    Name = "Diamond Push-Up",
                    VideoAdress = "/images/workoutGifs/Diamond-Push-up.gif",
                    Description = "Perform a push-up with your hands close together in a diamond shape to target triceps.",
                    Difficulty = 3,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Triceps",
                    Secondary_Musclegroup = "Chest"
                },

                new Exercise
                {
                    Name = "Double Arm Dumbbell Curl",
                    VideoAdress = "/images/workoutGifs/Double-Arm-Dumbbell-Curl.gif",
                    Description = "Curl both dumbbells simultaneously to work your biceps.",
                    Difficulty = 2,
                    Equipment = 1,
                    Type = "Strength",
                    Primary_Musclegroup = "Biceps",
                    Secondary_Musclegroup = "Forearms"
                },

                new Exercise
                {
                    Name = "Dumbbell 4 Ways Lateral Raise",
                    VideoAdress = "/images/workoutGifs/Dumbbell-4-Ways-Lateral-Raise.gif",
                    Description = "Perform lateral raises in four directions to engage all shoulder muscles.",
                    Difficulty = 3,
                    Equipment = 1,
                    Type = "Strength",
                    Primary_Musclegroup = "Shoulders",
                    Secondary_Musclegroup = "Upper Back"
                },

                new Exercise
                {
                    Name = "Dumbbell 6 Ways Raise",
                    VideoAdress = "/images/workoutGifs/Dumbbell-6-Ways-Raise.gif",
                    Description = "Raise dumbbells in six different directions to work all parts of the shoulders.",
                    Difficulty = 4,
                    Equipment = 1,
                    Type = "Strength",
                    Primary_Musclegroup = "Shoulders",
                    Secondary_Musclegroup = "Upper Back"
                },

                new Exercise
                {
                    Name = "Dumbbell Cossack Squat",
                    VideoAdress = "/images/workoutGifs/dumbbell-cossack-squat.gif",
                    Description = "Perform a deep side lunge holding a dumbbell to stretch and strengthen your lower body.",
                    Difficulty = 3,
                    Equipment = 1,
                    Type = "Strength",
                    Primary_Musclegroup = "Quadriceps",
                    Secondary_Musclegroup = "Adductors"
                },

                new Exercise
                {
                    Name = "Dumbbell Curl",
                    VideoAdress = "/images/workoutGifs/Dumbbell-Curl.gif",
                    Description = "Curl the dumbbells toward your shoulders to work your biceps.",
                    Difficulty = 2,
                    Equipment = 1,
                    Type = "Strength",
                    Primary_Musclegroup = "Biceps",
                    Secondary_Musclegroup = "Forearms"
                },

                new Exercise
                {
                    Name = "Dumbbell Deadlifts",
                    VideoAdress = "/images/workoutGifs/dumbbell-deadlifts.gif",
                    Description = "Lift dumbbells from the ground while keeping a straight back to work your posterior chain.",
                    Difficulty = 3,
                    Equipment = 1,
                    Type = "Strength",
                    Primary_Musclegroup = "Hamstrings",
                    Secondary_Musclegroup = "Lower Back"
                },

                new Exercise
                {
                    Name = "Dumbbell Devil Press",
                    VideoAdress = "/images/workoutGifs/Dumbbell-Devil-Press.gif",
                    Description = "Combine a burpee and a dumbbell press to work your full body.",
                    Difficulty = 5,
                    Equipment = 1,
                    Type = "Strength",
                    Primary_Musclegroup = "Full Body",
                    Secondary_Musclegroup = "Shoulders"
                },

                new Exercise
                {
                    Name = "Dumbbell Goblet Squat",
                    VideoAdress = "/images/workoutGifs/Dumbbell-Goblet-Squat.gif",
                    Description = "Hold a dumbbell close to your chest and squat deeply to target your legs.",
                    Difficulty = 2,
                    Equipment = 1,
                    Type = "Strength",
                    Primary_Musclegroup = "Quadriceps",
                    Secondary_Musclegroup = "Glutes"
                },

                new Exercise
                {
                    Name = "Dumbbell Lateral Raise",
                    VideoAdress = "/images/workoutGifs/Dumbbell-Lateral-Raise.gif",
                    Description = "Raise dumbbells to the sides to shoulder height to work your deltoid muscles.",
                    Difficulty = 2,
                    Equipment = 1,
                    Type = "Strength",
                    Primary_Musclegroup = "Shoulders",
                    Secondary_Musclegroup = "Upper Back"
                },

                new Exercise
                {
                    Name = "Dumbbell Lunge",
                    VideoAdress = "/images/workoutGifs/Dumbbell-Lunge.gif",
                    Description = "Step forward into a lunge while holding dumbbells to work your legs and glutes.",
                    Difficulty = 3,
                    Equipment = 1,
                    Type = "Strength",
                    Primary_Musclegroup = "Quadriceps",
                    Secondary_Musclegroup = "Glutes"
                },

                new Exercise
                {
                    Name = "Dumbbell Lunges",
                    VideoAdress = "/images/workoutGifs/dumbbell-lunges.gif",
                    Description = "Perform alternating lunges with dumbbells to strengthen your lower body.",
                    Difficulty = 3,
                    Equipment = 1,
                    Type = "Strength",
                    Primary_Musclegroup = "Quadriceps",
                    Secondary_Musclegroup = "Hamstrings"
                },

                new Exercise
                {
                    Name = "Dumbbell Push Press",
                    VideoAdress = "/images/workoutGifs/Dumbbell-Push-Press.gif",
                    Description = "Use leg drive to press dumbbells overhead, combining power and strength.",
                    Difficulty = 4,
                    Equipment = 1,
                    Type = "Strength",
                    Primary_Musclegroup = "Shoulders",
                    Secondary_Musclegroup = "Legs"
                },

                new Exercise
                {
                    Name = "Dumbbell Renegade Row",
                    VideoAdress = "/images/workoutGifs/dumbbell-renegade-row-1.gif",
                    Description = "Hold a plank position and row dumbbells alternately to target your back and core.",
                    Difficulty = 4,
                    Equipment = 1,
                    Type = "Strength",
                    Primary_Musclegroup = "Back",
                    Secondary_Musclegroup = "Core"
                },

                new Exercise
                {
                    Name = "Dumbbell Shoulder Press",
                    VideoAdress = "/images/workoutGifs/Dumbbell-Shoulder-Press.gif",
                    Description = "Press dumbbells overhead while seated to build shoulder strength.",
                    Difficulty = 3,
                    Equipment = 1,
                    Type = "Strength",
                    Primary_Musclegroup = "Shoulders",
                    Secondary_Musclegroup = "Triceps"
                },

                new Exercise
                {
                    Name = "Dumbbell Side Bend",
                    VideoAdress = "/images/workoutGifs/Dumbbell-Side-Bend.gif",
                    Description = "Bend sideways while holding a dumbbell to work your obliques.",
                    Difficulty = 2,
                    Equipment = 1,
                    Type = "Strength",
                    Primary_Musclegroup = "Obliques",
                    Secondary_Musclegroup = "Abdominals"
                },

                new Exercise
                {
                    Name = "Dumbbell Sumo Deadlift",
                    VideoAdress = "/images/workoutGifs/dumbbell-sumo-deadlift.gif",
                    Description = "Perform a deadlift with a wide stance, holding dumbbells to target your inner thighs and glutes.",
                    Difficulty = 3,
                    Equipment = 1,
                    Type = "Strength",
                    Primary_Musclegroup = "Glutes",
                    Secondary_Musclegroup = "Quadriceps"
                },

                new Exercise
                {
                    Name = "Dumbbell Upward Fly",
                    VideoAdress = "/images/workoutGifs/Dumbbell-Upward-Fly.gif",
                    Description = "Raise dumbbells upward in an arc to engage your chest and shoulders.",
                    Difficulty = 2,
                    Equipment = 1,
                    Type = "Strength",
                    Primary_Musclegroup = "Chest",
                    Secondary_Musclegroup = "Shoulders"
                },

                new Exercise
                {
                    Name = "Dynamic Chest Stretch",
                    VideoAdress = "/images/workoutGifs/Dynamic-Chest-Stretch.gif",
                    Description = "Extend your arms outward and bring them forward to stretch your chest dynamically.",
                    Difficulty = 1,
                    Equipment = null,
                    Type = "Stretching",
                    Primary_Musclegroup = "Chest",
                    Secondary_Musclegroup = "Shoulders"
                },

                new Exercise
                {
                    Name = "Floor Crunch",
                    VideoAdress = "/images/workoutGifs/Floor-Crunch.gif",
                    Description = "Lie on your back and lift your shoulders off the floor to work your abdominals.",
                    Difficulty = 1,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Abdominals",
                    Secondary_Musclegroup = null
                },

                new Exercise
                {
                    Name = "Forearm Push-Up",
                    VideoAdress = "/images/workoutGifs/Forearm-Push-up.gif",
                    Description = "Perform a push-up while resting on your forearms to engage the triceps and shoulders.",
                    Difficulty = 3,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Triceps",
                    Secondary_Musclegroup = "Shoulders"
                },

                new Exercise
                {
                    Name = "Hammer Curl",
                    VideoAdress = "/images/workoutGifs/Hammer-Curl.gif",
                    Description = "Curl dumbbells with a neutral grip to target the biceps and forearms.",
                    Difficulty = 2,
                    Equipment = 1,
                    Type = "Strength",
                    Primary_Musclegroup = "Biceps",
                    Secondary_Musclegroup = "Forearms"
                },

                new Exercise
                {
                    Name = "Hanging Knee Raises",
                    VideoAdress = "/images/workoutGifs/Hanging-Knee-Raises.gif",
                    Description = "Hang from a bar and raise your knees to engage your core.",
                    Difficulty = 3,
                    Equipment = 2,
                    Type = "Strength",
                    Primary_Musclegroup = "Abdominals",
                    Secondary_Musclegroup = "Hip Flexors"
                },

                new Exercise
                {
                    Name = "Hanging Side Knee Raises",
                    VideoAdress = "/images/workoutGifs/Hanging-Side-Knee-Raises.gif",
                    Description = "Hang from a bar and raise your knees to the side to target obliques.",
                    Difficulty = 4,
                    Equipment = 2,
                    Type = "Strength",
                    Primary_Musclegroup = "Obliques",
                    Secondary_Musclegroup = "Hip Flexors"
                },

                new Exercise
                {
                    Name = "High Knee Squat",
                    VideoAdress = "/images/workoutGifs/High-Knee-Squat.gif",
                    Description = "Perform a squat and bring your knees up high alternately to engage your core and legs.",
                    Difficulty = 3,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Quadriceps",
                    Secondary_Musclegroup = "Core"
                },

                new Exercise
                {
                    Name = "Incline Push-Up",
                    VideoAdress = "/images/workoutGifs/Incline-Push-Up.gif",
                    Description = "Perform a push-up with your hands elevated on a surface to reduce difficulty.",
                    Difficulty = 2,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Chest",
                    Secondary_Musclegroup = "Triceps"
                },

                new Exercise
                {
                    Name = "Jump Squat",
                    VideoAdress = "/images/workoutGifs/Jump-Squat.gif",
                    Description = "Perform a squat followed by an explosive jump to build lower body power.",
                    Difficulty = 4,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Quadriceps",
                    Secondary_Musclegroup = "Glutes"
                },

                new Exercise
                {
                    Name = "Kneeling Push-Up",
                    VideoAdress = "/images/workoutGifs/Kneeling-Push-up.gif",
                    Description = "Perform a push-up from your knees to reduce difficulty and focus on the upper body.",
                    Difficulty = 1,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Chest",
                    Secondary_Musclegroup = "Triceps"
                },

                new Exercise
                {
                    Name = "Kneeling T-Spine Rotation",
                    VideoAdress = "/images/workoutGifs/Kneeling-T-spine-Rotation.gif",
                    Description = "Perform a rotation from a kneeling position to improve thoracic spine mobility.",
                    Difficulty = 1,
                    Equipment = null,
                    Type = "Stretching",
                    Primary_Musclegroup = "Back",
                    Secondary_Musclegroup = "Shoulders"
                },

                new Exercise
                {
                    Name = "Knee Push-Up",
                    VideoAdress = "/images/workoutGifs/Knee-Push-Up.gif",
                    Description = "A modified push-up performed from the knees to target the chest and triceps.",
                    Difficulty = 1,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Chest",
                    Secondary_Musclegroup = "Triceps"
                },

                new Exercise
                {
                    Name = "L-Sit",
                    VideoAdress = "/images/workoutGifs/L-Sit.gif",
                    Description = "Hold your body in an 'L' position, engaging your core and hip flexors.",
                    Difficulty = 5,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Abdominals",
                    Secondary_Musclegroup = "Hip Flexors"
                },

                new Exercise
                {
                    Name = "Lying Knee Raise",
                    VideoAdress = "/images/workoutGifs/Lying-Knee-Raise.gif",
                    Description = "Lie flat and raise your knees toward your chest to engage your lower abs.",
                    Difficulty = 2,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Abdominals",
                    Secondary_Musclegroup = "Hip Flexors"
                },

                new Exercise
                {
                    Name = "Lying Leg Raise",
                    VideoAdress = "/images/workoutGifs/Lying-Leg-Raise.gif",
                    Description = "Lift your legs while lying down to strengthen your core and lower abs.",
                    Difficulty = 3,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Abdominals",
                    Secondary_Musclegroup = "Hip Flexors"
                },

                new Exercise
                {
                    Name = "Lying Scissor Kick",
                    VideoAdress = "/images/workoutGifs/Lying-Scissor-Kick.gif",
                    Description = "Perform alternating leg movements to target the lower abs and hip flexors.",
                    Difficulty = 3,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Abdominals",
                    Secondary_Musclegroup = "Hip Flexors"
                },

                new Exercise
                {
                    Name = "Medicine Ball Overhead Slam",
                    VideoAdress = "/images/workoutGifs/Medicine-ball-Overhead-Slam-exercise.gif",
                    Description = "Lift a medicine ball overhead and slam it to the ground to build power.",
                    Difficulty = 4,
                    Equipment = 4,
                    Type = "Strength",
                    Primary_Musclegroup = "Shoulders",
                    Secondary_Musclegroup = "Core"
                },

                new Exercise
                {
                    Name = "Modified Hindu Push-Up",
                    VideoAdress = "/images/workoutGifs/Modified-Hindu-Push-up.gif",
                    Description = "A dynamic push-up variation focusing on shoulder and chest mobility.",
                    Difficulty = 3,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Chest",
                    Secondary_Musclegroup = "Shoulders"
                },

                new Exercise
                {
                    Name = "Mountain Climber",
                    VideoAdress = "/images/workoutGifs/Mountain-climber.gif",
                    Description = "Alternate bringing your knees to your chest while in a plank position for cardio and core work.",
                    Difficulty = 2,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Abdominals",
                    Secondary_Musclegroup = "Legs"
                },

                new Exercise
                {
                    Name = "Muscle-Up on Vertical Bar",
                    VideoAdress = "/images/workoutGifs/Muscle-up-vertical-bar.gif",
                    Description = "An advanced pull-up transitioning into a dip over a vertical bar.",
                    Difficulty = 5,
                    Equipment = 2,
                    Type = "Strength",
                    Primary_Musclegroup = "Back",
                    Secondary_Musclegroup = "Triceps"
                },

                new Exercise
                {
                    Name = "Narrow Grip Wall Push-Up",
                    VideoAdress = "/images/workoutGifs/Narrow-Grip-Wall-Push-Up.gif",
                    Description = "Perform a wall push-up with a narrow grip to target the triceps.",
                    Difficulty = 1,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Triceps",
                    Secondary_Musclegroup = "Chest"
                },

                new Exercise
                {
                    Name = "Navy Seal Burpee",
                    VideoAdress = "/images/workoutGifs/Navy-Seal-Burpee.gif",
                    Description = "A burpee variation that includes a push-up and a dynamic lunge.",
                    Difficulty = 5,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Full Body",
                    Secondary_Musclegroup = "Legs"
                },

                new Exercise
                {
                    Name = "Pike Push-Up",
                    VideoAdress = "/images/workoutGifs/Pike-Push-up.gif",
                    Description = "Perform a push-up with hips elevated to target the shoulders.",
                    Difficulty = 3,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Shoulders",
                    Secondary_Musclegroup = "Triceps"
                },

                new Exercise
                {
                    Name = "Plank with Arm and Leg Lift",
                    VideoAdress = "/images/workoutGifs/Plank-with-Arm-and-Leg-Lift.gif",
                    Description = "Hold a plank position and alternately lift an arm and the opposite leg.",
                    Difficulty = 3,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Core",
                    Secondary_Musclegroup = "Shoulders"
                },

                new Exercise
                {
                    Name = "Power Lunge",
                    VideoAdress = "/images/workoutGifs/power-lunge.gif",
                    Description = "Perform a dynamic lunge with an explosive jump to engage your legs.",
                    Difficulty = 4,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Quadriceps",
                    Secondary_Musclegroup = "Glutes"
                },

                new Exercise
                {
                    Name = "Push-Up",
                    VideoAdress = "/images/workoutGifs/Push-Up.gif",
                    Description = "A classic exercise targeting the chest, shoulders, and triceps.",
                    Difficulty = 2,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Chest",
                    Secondary_Musclegroup = "Triceps"
                },

                new Exercise
                {
                    Name = "Push-Up Medicine Ball",
                    VideoAdress = "/images/workoutGifs/Push-Up-Medicine-Ball.gif",
                    Description = "Perform a push-up with one hand on a medicine ball to increase difficulty and stability.",
                    Difficulty = 4,
                    Equipment = 4,
                    Type = "Strength",
                    Primary_Musclegroup = "Chest",
                    Secondary_Musclegroup = "Shoulders"
                },

                new Exercise
                {
                    Name = "Push-Up Plus",
                    VideoAdress = "/images/workoutGifs/Push-Up-Plus.gif",
                    Description = "Perform a push-up and add an extra shoulder protraction at the top to engage the serratus anterior.",
                    Difficulty = 3,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Chest",
                    Secondary_Musclegroup = "Shoulders"
                },

                new Exercise
                {
                    Name = "Push-Up Toe Touch",
                    VideoAdress = "/images/workoutGifs/Push-up-Toe-Touch.gif",
                    Description = "Perform a push-up and touch your opposite foot with one hand between reps.",
                    Difficulty = 4,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Chest",
                    Secondary_Musclegroup = "Core"
                },

                new Exercise
                {
                    Name = "Push-Up to Renegade Row",
                    VideoAdress = "/images/workoutGifs/Push-Up-to-Renegade-Row.gif",
                    Description = "Perform a push-up followed by a dumbbell row to engage your back and core.",
                    Difficulty = 4,
                    Equipment = 1,
                    Type = "Strength",
                    Primary_Musclegroup = "Chest",
                    Secondary_Musclegroup = "Back"
                },

                new Exercise
                {
                    Name = "Push-Up with Rotation",
                    VideoAdress = "/images/workoutGifs/push-up-with-rotation.gif",
                    Description = "Perform a push-up and rotate into a side plank position to engage core muscles.",
                    Difficulty = 3,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Chest",
                    Secondary_Musclegroup = "Core"
                },

                new Exercise
                {
                    Name = "Resistance Band Toe Touch",
                    VideoAdress = "/images/workoutGifs/Resistance-Band-Toe-Touch.gif",
                    Description = "Perform a toe touch while maintaining resistance band tension for added difficulty.",
                    Difficulty = 3,
                    Equipment = 3,
                    Type = "Strength",
                    Primary_Musclegroup = "Abdominals",
                    Secondary_Musclegroup = "Hip Flexors"
                },

                new Exercise
                {
                    Name = "Reverse Chest Stretch",
                    VideoAdress = "/images/workoutGifs/Reverse-Chest-Stretch.gif",
                    Description = "Stretch your chest by clasping your hands behind your back and pulling them downward.",
                    Difficulty = 1,
                    Equipment = null,
                    Type = "Stretching",
                    Primary_Musclegroup = "Chest",
                    Secondary_Musclegroup = "Shoulders"
                },

                new Exercise
                {
                    Name = "Reverse Dips",
                    VideoAdress = "/images/workoutGifs/Reverse-Dips.gif",
                    Description = "Perform dips using a bench to target the triceps and chest.",
                    Difficulty = 2,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Triceps",
                    Secondary_Musclegroup = "Chest"
                },

                new Exercise
                {
                    Name = "Reverse Push-Up",
                    VideoAdress = "/images/workoutGifs/Reverse-Push-up.gif",
                    Description = "A push-up variation where you press back towards your legs in a reverse motion.",
                    Difficulty = 3,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Shoulders",
                    Secondary_Musclegroup = "Triceps"
                },

                new Exercise
                {
                    Name = "Seated Chest Stretch",
                    VideoAdress = "/images/workoutGifs/Seated-Chest-Stretch.gif",
                    Description = "Sit and extend your arms back to stretch the chest muscles.",
                    Difficulty = 1,
                    Equipment = null,
                    Type = "Stretching",
                    Primary_Musclegroup = "Chest",
                    Secondary_Musclegroup = "Shoulders"
                },

                new Exercise
                {
                    Name = "Seated Dumbbell Alternating Curl",
                    VideoAdress = "/images/workoutGifs/Seated-dumbbell-alternating-curl.gif",
                    Description = "Seated bicep curls alternating between arms to build arm strength.",
                    Difficulty = 2,
                    Equipment = 1,
                    Type = "Strength",
                    Primary_Musclegroup = "Biceps",
                    Secondary_Musclegroup = "Forearms"
                },

                new Exercise
                {
                    Name = "Seated Rear Lateral Dumbbell Raise",
                    VideoAdress = "/images/workoutGifs/Seated-Rear-Lateral-Dumbbell-Raise.gif",
                    Description = "Raise dumbbells laterally while seated, targeting the rear delts.",
                    Difficulty = 3,
                    Equipment = 1,
                    Type = "Strength",
                    Primary_Musclegroup = "Shoulders",
                    Secondary_Musclegroup = "Upper Back"
                },

                new Exercise
                {
                    Name = "Seated Side Crunches",
                    VideoAdress = "/images/workoutGifs/Seated-Side-Crunches.gif",
                    Description = "Crunch to the side while seated to engage oblique muscles.",
                    Difficulty = 2,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Obliques",
                    Secondary_Musclegroup = "Abdominals"
                },

                new Exercise
                {
                    Name = "Seated Zottoman Curl",
                    VideoAdress = "/images/workoutGifs/Seated-Zottoman-Curl.gif",
                    Description = "Perform a seated bicep curl with a twist to target forearms and biceps.",
                    Difficulty = 3,
                    Equipment = 1,
                    Type = "Strength",
                    Primary_Musclegroup = "Biceps",
                    Secondary_Musclegroup = "Forearms"
                },

                new Exercise
                {
                    Name = "Shoulder Stretch Behind Back",
                    VideoAdress = "/images/workoutGifs/Shoulder-Stretch-Behind-Back.gif",
                    Description = "Stretch your shoulders by reaching behind your back and pulling gently.",
                    Difficulty = 1,
                    Equipment = null,
                    Type = "Stretching",
                    Primary_Musclegroup = "Shoulders",
                    Secondary_Musclegroup = "Chest"
                },

                new Exercise
                {
                    Name = "Single Arm Push-Up",
                    VideoAdress = "/images/workoutGifs/Single-Arm-Push-up.gif",
                    Description = "Perform a push-up using only one arm for advanced strength training.",
                    Difficulty = 5,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Chest",
                    Secondary_Musclegroup = "Core"
                },

                new Exercise
                {
                    Name = "Snap Jumps",
                    VideoAdress = "/images/workoutGifs/SNAP-JUMPS.gif",
                    Description = "Explosive jumps from a squat position to engage full body muscles.",
                    Difficulty = 4,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Legs",
                    Secondary_Musclegroup = "Core"
                },

                new Exercise
                {
                    Name = "Standing Dumbbell Overhead Press",
                    VideoAdress = "/images/workoutGifs/Standing-Dumbbell-Overhead-Press.gif",
                    Description = "Press dumbbells overhead while standing to build shoulder strength.",
                    Difficulty = 3,
                    Equipment = 1,
                    Type = "Strength",
                    Primary_Musclegroup = "Shoulders",
                    Secondary_Musclegroup = "Triceps"
                },

                new Exercise
                {
                    Name = "Standing Leg Circles",
                    VideoAdress = "/images/workoutGifs/Standing-Leg-Circles.gif",
                    Description = "Move your legs in circular motions while standing to improve mobility.",
                    Difficulty = 1,
                    Equipment = null,
                    Type = "Stretching",
                    Primary_Musclegroup = "Hip Flexors",
                    Secondary_Musclegroup = "Glutes"
                },

                new Exercise
                {
                    Name = "Standing One Arm Chest Stretch",
                    VideoAdress = "/images/workoutGifs/Standing-one-arm-chest-stretch.gif",
                    Description = "Stretch your chest by extending one arm to the side while standing.",
                    Difficulty = 1,
                    Equipment = null,
                    Type = "Stretching",
                    Primary_Musclegroup = "Chest",
                    Secondary_Musclegroup = "Shoulders"
                },

                new Exercise
                {
                    Name = "Standing Rotation",
                    VideoAdress = "/images/workoutGifs/Standing-Rotation.gif",
                    Description = "Rotate your torso while standing to engage core and improve flexibility.",
                    Difficulty = 1,
                    Equipment = null,
                    Type = "Stretching",
                    Primary_Musclegroup = "Core",
                    Secondary_Musclegroup = "Obliques"
                },

                new Exercise
                {
                    Name = "Standing Toe to Touch",
                    VideoAdress = "/images/workoutGifs/Standing-Toe-To-Touch.gif",
                    Description = "Bend forward and touch your toes to stretch your hamstrings.",
                    Difficulty = 1,
                    Equipment = null,
                    Type = "Stretching",
                    Primary_Musclegroup = "Hamstrings",
                    Secondary_Musclegroup = "Lower Back"
                },

                new Exercise
                {
                    Name = "Static Lunge",
                    VideoAdress = "/images/workoutGifs/Static-Lunge.gif",
                    Description = "Hold a lunge position to engage your legs and glutes statically.",
                    Difficulty = 2,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Quadriceps",
                    Secondary_Musclegroup = "Glutes"
                },

                new Exercise
                {
                    Name = "Wall Ball",
                    VideoAdress = "/images/workoutGifs/wall-ball.gif",
                    Description = "Throw a medicine ball against the wall while performing a squat.",
                    Difficulty = 3,
                    Equipment = 4,
                    Type = "Strength",
                    Primary_Musclegroup = "Legs",
                    Secondary_Musclegroup = "Shoulders"
                },

                new Exercise
                {
                    Name = "Wall Push-Ups",
                    VideoAdress = "/images/workoutGifs/Wall-Push-ups.gif",
                    Description = "Push against a wall to perform a modified push-up.",
                    Difficulty = 1,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Chest",
                    Secondary_Musclegroup = "Triceps"
                },

                new Exercise
                {
                    Name = "Wall Walk",
                    VideoAdress = "/images/workoutGifs/wall-walk-muscles.gif",
                    Description = "Walk your feet up the wall while maintaining a plank to build shoulder strength.",
                    Difficulty = 4,
                    Equipment = null,
                    Type = "Strength",
                    Primary_Musclegroup = "Shoulders",
                    Secondary_Musclegroup = "Core"
                },

                new Exercise
                {
                    Name = "Weighted Hanging Knee Raises",
                    VideoAdress = "/images/workoutGifs/weighted-hanging-knee-raises.gif",
                    Description = "Hang from a bar and raise your knees while holding a weight for added difficulty.",
                    Difficulty = 4,
                    Equipment = 2,
                    Type = "Strength",
                    Primary_Musclegroup = "Abdominals",
                    Secondary_Musclegroup = "Hip Flexors"
                },

                new Exercise
                {
                    Name = "Wrist Circles Stretch",
                    VideoAdress = "/images/workoutGifs/Wrist-Circles-Stretch.gif",
                    Description = "Rotate your wrists in circles to improve mobility and reduce tension.",
                    Difficulty = 1,
                    Equipment = null,
                    Type = "Stretching",
                    Primary_Musclegroup = "Forearms",
                    Secondary_Musclegroup = "Wrists"
                },

                new Exercise
                {
                    Name = "Zottoman Curl",
                    VideoAdress = "/images/workoutGifs/zottoman-curl.gif",
                    Description = "A curl variation that targets both the biceps and forearms through a twist.",
                    Difficulty = 3,
                    Equipment = 1,
                    Type = "Strength",
                    Primary_Musclegroup = "Biceps",
                    Secondary_Musclegroup = "Forearms"
                }
            };
            db.Exercises.AddRange(exercises);
            db.SaveChanges();
        } 
    }
    
}


