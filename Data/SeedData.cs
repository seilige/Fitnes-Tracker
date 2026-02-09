namespace FitnesTracker;

public static class SeedData
{
    public static void Initialize(ApplicationDbContext context)
    {
        if (context.Users.Any() || context.WorkoutSessions.Any())
        {
            return; // БД уже заполнена
        }

        var users = new User[]
        {
            new User { Name = "testuser", Lastname = "tes" }
        };
        context.Users.AddRange(users);
        context.SaveChanges();

        // Создаем упражнения
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
                Level = 0, // или ProgramLevel.Beginner если enum
                Category = 0, // или ProgramCategory.Strength если enum
                WorkoutType = 0 // или WorkoutType.FullBody если enum
            }
        };

        context.StandardPrograms.AddRange(programs);
        context.SaveChanges();

        // Добавь связь юзера с программой (MTM таблица)
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

        // Добавь упражнения в программу (MTM таблица)
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



        // Создаем сессию
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

        // Создаем сеты
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
