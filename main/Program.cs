using Microsoft.EntityFrameworkCore;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.OpenApi.Models;
using Serilog.Events;
using Serilog;

namespace FitnesTracker;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("logs/app-.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
            .CreateLogger();

        try
        {
            var builder = WebApplication.CreateBuilder(args);
            var corsOpts = builder.Configuration.GetSection("Cors").Get<CorsOptions>()!;
            var rateLimiterOpts = builder.Configuration.GetSection("RateLimiter").Get<RateLimiterOptions>()!;

            builder.Host.UseSerilog();

            builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection"));

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
            builder.Services.AddScoped<IAuthentication, AuthenticationService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddAutoMapper(typeof(FitnesTracker.Mapper).Assembly);

            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();

            builder.Services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Fitness Tracker API",
                    Version = "v1",
                    Description = "API for managing fitness programs, exercises and users",
                    Contact = new OpenApiContact
                    {
                        Name = "N",
                        Email = "N@email.com"
                    }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });
                
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference 
                            { 
                                Type = ReferenceType.SecurityScheme, 
                                Id = "Bearer" 
                            }
                        },
                        Array.Empty<string>()
                    }
                });
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
                options.AddPolicy(corsOpts.PolicyName, policy =>
                {
                    policy.WithOrigins(corsOpts.AllowedOrigins)
                        .WithMethods(corsOpts.AllowedMethods)
                        .WithHeaders(corsOpts.AllowedHeaders);
                });
            });

            builder.Services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter(rateLimiterOpts.PolicyName, opt =>
                {
                    opt.Window = TimeSpan.FromMinutes(rateLimiterOpts.WindowMinutes);
                    opt.PermitLimit = rateLimiterOpts.PermitLimit;
                    opt.QueueLimit = rateLimiterOpts.QueueLimit;
                });

                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = 429;
                    await context.HttpContext.Response.WriteAsync("Too many requests, please wait.");
                };
            });

            var app = builder.Build();
            app.UseSerilogRequestLogging();

            app.UseCors(corsOpts.PolicyName);

            // custom data seed
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                SeedData.Initialize(context);
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();

            app.MapHealthChecks("/health");

            app.UseSwagger();
            app.UseSwaggerUI();
        
            app.UseAuthentication();

            app.UseMiddleware<EmailConfirmationMiddleware>();

            app.UseAuthorization();
        

            app.UseRateLimiter();

            app.MapControllers();

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
