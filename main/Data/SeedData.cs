namespace FitnesTracker;

public static class SeedData
{
    public static void Initialize(ApplicationDbContext context)
    {
        // if minimal seed exists
        if (context.Users.Any() || context.WorkoutSessions.Any())
        {
            return;
        }

        var users = new User[]
        {
            new User { Name = "testuser", Lastname = "tes" }
        };
    
        context.Users.AddRange(users);
        context.SaveChanges();

        var exercises = new Exercise[]
        {
            new Exercise { Title = "Bench Press", Sets = 3, Reps = 10, MuscleGroup = MuscleGroup.Chest },
            new Exercise { Title = "Squat", Sets = 4, Reps = 8, MuscleGroup = MuscleGroup.Legs },
            new Exercise { Title = "Deadlift", Sets = 3, Reps = 5, MuscleGroup = MuscleGroup.Back }
        };

        context.Exercises.AddRange(exercises);
        context.SaveChanges();

        var programs = new StandardProgram[]
        {
            new StandardProgram 
            { 
                Title = "Beginner Program",
                Description = "Basic program for beginners",
                Level = 0, // enum level
                Category = 0, // enum
                WorkoutType = 0 // enum
            }
        };

        context.StandardPrograms.AddRange(programs);
        context.SaveChanges();

        // standart program with no custom progamm
        var userPrograms = new UserStandardProgram[]
        {
            new UserStandardProgram 
            { 
                IdUser = users[0].IdUser,
                ProgId = programs[0].ProgId
            }
        };
        context.UserStandardPrograms.AddRange(userPrograms);
        context.SaveChanges();

        var programExercises = new StandardProgramExercise[]
        {
            new StandardProgramExercise 
            { 
                ProgId = programs[0].ProgId,
                ExId = exercises[0].ExId 
            },
            new StandardProgramExercise 
            { 
                ProgId = programs[0].ProgId,
                ExId = exercises[1].ExId 
            }
        };
        context.StandardProgramExercises.AddRange(programExercises);
        context.SaveChanges();


        var sessions = new WorkoutSession[]
        {
            new WorkoutSession 
            { 
                UserId = users[0].IdUser,
                StandardProgramId = programs[0].ProgId,
                Date = DateTime.UtcNow,
                Status = WorkoutStatus.InProgress
            }
        };
        context.WorkoutSessions.AddRange(sessions);
        context.SaveChanges();
    
        var sets = new WorkoutExerciseSet[]
        {
            new WorkoutExerciseSet 
            { 
                WorkoutSessionId = sessions[0].SessionId,
                ExerciseId = exercises[0].ExId,
                Sets = 3,
                Reps = 10,
                Weight = 60
            }
        };
        context.WorkoutExerciseSets.AddRange(sets);
        context.SaveChanges();
    }
}
