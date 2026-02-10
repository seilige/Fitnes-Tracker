using Microsoft.EntityFrameworkCore;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace FitnesTracker;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddScoped<ICustomProgramRepository, CustomProgramRepository>();
        builder.Services.AddScoped<IStandardProgramRepository, StandardProgramRepository>();
        builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IWorkoutSessionRepository, WorkoutSessionRepository>();
        builder.Services.AddScoped<IWorkoutExerciseSetRepository, WorkoutExerciseSetRepository>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IStandardProgramService, StandardProgramService>();
        builder.Services.AddScoped<ICustomProgramService, CustomProgramService>();
        builder.Services.AddScoped<IExerciseService, ExerciseService>();
        builder.Services.AddScoped<IWorkoutSessionService, WorkoutSessionService>();
        builder.Services.AddScoped<IWorkoutExerciseService, WorkoutExerciseService>();

        builder.Services.AddAutoMapper(typeof(FitnesTracker.Mapper).Assembly);

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { 
                Title = "API", 
                Version = "v1",
                Description = "API"
            });
    
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));


        builder.Services.AddSwaggerGen(c =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors());

        var app = builder.Build();

        // custom data seed
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            SeedData.Initialize(context);
        }


        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseHttpsRedirection();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
