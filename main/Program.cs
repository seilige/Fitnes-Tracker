using Microsoft.EntityFrameworkCore;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Net.Http.Headers;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.RateLimiting;

namespace FitnesTracker;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<GlobalExceptionFilter>();
        });

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
        builder.Services.AddScoped<IAuthentication, Authentication>();

        builder.Services.AddAutoMapper(typeof(FitnesTracker.Mapper).Assembly);

        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();

        builder.Services.AddSwaggerGen(c =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                };
            });

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors());


        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowClient", policy =>
            {
                policy.WithOrigins("http://127.0.0.1:5501") // allow the client from this port read servers responce
                    .WithMethods("GET", "POST", "PUT", "DELETE") // allow this http methods
                    .WithHeaders(HeaderNames.ContentType, "x-custom-header");
                policy.WithOrigins("http://127.0.0.1:5501", "http://localhost:5501");
            });
        });

        builder.Services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("Fixed", opt => // in each windows client can request N times
            {
                opt.Window = TimeSpan.FromMinutes(1);
                opt.PermitLimit = 2;
                opt.QueueLimit = 0;
            });

            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.StatusCode = 429;
                await context.HttpContext.Response.WriteAsync("To many requests, please wait.");
            };
        });

        var app = builder.Build();

        app.UseCors("AllowClient");

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
    
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.UseRateLimiter();

        app.Run();
    }
}
