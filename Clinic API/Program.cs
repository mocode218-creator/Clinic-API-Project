using Clinic_DataAccess.Extensions;
using Clinic_BusinessLayer.Extensions;
using Clinic_API.Extensions;
using Scalar.AspNetCore;
using Serilog;

// 1. إعداد Serilog
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

    // 3. سجّل الـ DataAccess Layer (DbContext + Repositories + UnitOfWork)
    builder.Services.AddDataAccessLayer(builder.Configuration);
    builder.Services.AddBusinessLayer();

    // 4. سجّل الـ Controllers
    builder.Services.AddControllers();

    // 5. OpenAPI (طريقة .NET 10)
    builder.Services.AddOpenApi();

    var app = builder.Build();

    // ✅ أول حاجة — عشان يمسك كل الأخطاء
    app.UseExceptionHandling();

    // 6. الـ Pipeline
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
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