using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FitnesTracker;

public class ApplicationDbContext : DbContext
{
    // Standard models
    public DbSet<User> Users { get; set; }
    public DbSet<StandardProgram> StandardPrograms { get; set; }
    public DbSet<CustomProgram> CustomPrograms { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    
    // Linking models
    public DbSet<UserStandardProgram> UserStandardPrograms { get; set; }
    public DbSet<CustomProgramUser> CustomProgramUsers { get; set; }
    public DbSet<StandardProgramExercise> StandardProgramExercises { get; set; }
    public DbSet<CustomProgramExercise> CustomProgramExercises { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure primary keys
        modelBuilder.Entity<User>()
            .HasKey(x => x.IdUser);

        modelBuilder.Entity<StandardProgram>()
            .HasKey(x => x.ProgId);

        modelBuilder.Entity<CustomProgram>()
            .HasKey(x => x.CustProgId);

        modelBuilder.Entity<Exercise>()
            .HasKey(x => x.ExId);

        modelBuilder.Entity<UserStandardProgram>()
            .HasKey(x => new {x.IdUser, x.ProgId});

        modelBuilder.Entity<CustomProgramUser>()
            .HasKey(x => new {x.IdUser, x.CustProgId});

        modelBuilder.Entity<StandardProgramExercise>()
            .HasKey(x => new {x.ProgId, x.ExId});

        modelBuilder.Entity<CustomProgramExercise>()
            .HasKey(x => new {x.CustProgId, x.ExId});

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
            .HasForeignKey(x => x.CustProgId);

        modelBuilder.Entity<StandardProgramExercise>()
            .HasOne(x => x.StandardProgram)
            .WithMany()
            .HasForeignKey(x => x.ProgId);

        modelBuilder.Entity<StandardProgramExercise>()
            .HasOne(x => x.Exercise)
            .WithMany()
            .HasForeignKey(x => x.ExId);

        modelBuilder.Entity<CustomProgramExercise>()
            .HasOne(x => x.CustomProgram)
            .WithMany()
            .HasForeignKey(x => x.CustProgId);

        modelBuilder.Entity<CustomProgramExercise>()
            .HasOne(x => x.Exercise)
            .WithMany()
            .HasForeignKey(x => x.ExId);
    }
}