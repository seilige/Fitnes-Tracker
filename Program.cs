using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace FitnesTracker;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        builder.Services.AddSwaggerGen(c =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
