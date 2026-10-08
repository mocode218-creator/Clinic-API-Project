using Clinic_DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;

// 1. إعداد Serilog (قبل أي حاجة)
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/clinic-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Starting Clinic API...");

    var builder = WebApplication.CreateBuilder(args);

    // 2. اربط Serilog بالـ Host
    builder.Host.UseSerilog();

    // 3. سجّل الـ DbContext
    builder.Services.AddDbContext<ClinicDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    // 4. سجّل الـ Controllers
    builder.Services.AddControllers();

    // 5. OpenAPI (طريقة .NET 10)
    builder.Services.AddOpenApi();

    var app = builder.Build();

    // 6. الـ Pipeline
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application crashed unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}