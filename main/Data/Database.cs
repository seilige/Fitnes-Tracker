using Microsoft.EntityFrameworkCore;

namespace FitnesTracker;

public class ApplicationDbContext : DbContext
{
    // Standard models
    public DbSet<User> Users { get; set; }
    public DbSet<StandardProgram> StandardPrograms { get; set; }
    public DbSet<CustomProgram> CustomPrograms { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    // Linking models
    public DbSet<UserStandardProgram> UserStandardPrograms { get; set; }
    public DbSet<CustomProgramUser> CustomProgramUsers { get; set; }
    public DbSet<StandardProgramExercise> StandardProgramExercises { get; set; }
    public DbSet<CustomProgramExercise> CustomProgramExercises { get; set; }
    public DbSet<WorkoutSession> WorkoutSessions { get; set; }
    public DbSet<WorkoutExerciseSet> WorkoutExerciseSets { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure primary keys
        modelBuilder.Entity<User>()
            .HasKey(x => x.UserId);

        modelBuilder.Entity<StandardProgram>()
            .HasKey(x => x.ProgramId);

        modelBuilder.Entity<CustomProgram>()
            .HasKey(x => x.CustomProgramId);

        modelBuilder.Entity<Exercise>()
            .HasKey(x => x.ExerciseId);

        modelBuilder.Entity<UserStandardProgram>()
            .HasKey(x => new {x.IdUser, x.ProgId});

        modelBuilder.Entity<CustomProgramUser>()
            .HasKey(x => new {x.IdUser, x.CustomProgramId});

        modelBuilder.Entity<StandardProgramExercise>()
            .HasKey(x => new {x.ProgramId, x.ExerciseId});

        modelBuilder.Entity<CustomProgramExercise>()
            .HasKey(x => new {x.CustomProgramId, x.ExerciseId});

        // Connection between objects configuration
        modelBuilder.Entity<CustomProgram>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.CreatorId);

        modelBuilder.Entity<UserStandardProgram>()
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.IdUser);

        modelBuilder.Entity<UserStandardProgram>()
            .HasOne(x => x.StandardProgram)
            .WithMany()
            .HasForeignKey(x => x.ProgId);

        modelBuilder.Entity<CustomProgramUser>() // Call to customProgramUser
            .HasOne(x => x.User) // connect customProg to user
            .WithMany()
            .HasForeignKey(x => x.IdUser); // Link via IdUser

        modelBuilder.Entity<CustomProgramUser>()
            .HasOne(x => x.CustomProgram)
            .WithMany()
            .HasForeignKey(x => x.CustomProgramId);

        modelBuilder.Entity<StandardProgramExercise>()
            .HasOne(x => x.StandardProgram)
            .WithMany()
            .HasForeignKey(x => x.ProgramId);

        modelBuilder.Entity<StandardProgramExercise>()
            .HasOne(x => x.Exercise)
            .WithMany()
            .HasForeignKey(x => x.ExerciseId);

        modelBuilder.Entity<CustomProgramExercise>()
            .HasOne(x => x.CustomProgram)
            .WithMany()
            .HasForeignKey(x => x.CustomProgramId);

        modelBuilder.Entity<CustomProgramExercise>()
            .HasOne(x => x.Exercise)
            .WithMany()
            .HasForeignKey(x => x.ExerciseId);

        // Workout session and exercise conntection:
        modelBuilder.Entity<WorkoutExerciseSet>()
            .HasOne(x => x.WorkoutSession)
            .WithMany(x => x.WorkoutExerciseSets)
            .HasForeignKey(x => x.WorkoutSessionId);
    
        modelBuilder.Entity<WorkoutExerciseSet>()
            .HasOne(x => x.Exercise)
            .WithMany(x => x.WorkoutExerciseSets)
            .HasForeignKey(x => x.ExerciseId);

        modelBuilder.Entity<WorkoutSession>()
            .HasOne(x => x.User)
            .WithMany(x => x.WorkoutSessions)
            .HasForeignKey(x => x.UserId);
        
        modelBuilder.Entity<WorkoutSession>()
            .Property(w => w.WorkoutSessionId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<WorkoutSession>()
            .HasOne(x => x.User)
            .WithMany(x => x.WorkoutSessions)
            .HasForeignKey(x => x.UserId);

        modelBuilder.Entity<WorkoutSession>()
            .HasOne(x => x.StandardProgram)
            .WithMany()
            .HasForeignKey(x => x.StandardProgramId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<WorkoutSession>()
            .HasOne(x => x.CustomProgram)
            .WithMany()
            .HasForeignKey(x => x.CustomProgramId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<WorkoutSession>()
            .HasKey(x => x.WorkoutSessionId);
    }
}
