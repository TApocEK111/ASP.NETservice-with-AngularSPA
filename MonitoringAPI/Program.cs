
using MonitoringAPI.Repositories;

namespace MonitoringAPI;

public class Program
{
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // Resolve CORS restriction problem for testing with localhost:4200.
        var AllowSpecificOrigins = "_myAllowSpecificOrigins";
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: AllowSpecificOrigins,
                              policy =>
                              {
                                  policy.WithOrigins("http://localhost:4200").WithMethods("GET", "DELETE");
                              });
        });

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton<DevicesMemoryRepository>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();

        app.UseCors(AllowSpecificOrigins); // Enables policy from above

        app.MapControllers();

        app.Run();
    }
}
