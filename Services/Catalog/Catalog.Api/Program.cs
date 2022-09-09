using Catalog.Api.Data;
using Catalog.Api.Extensions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddDbContext<CatalogContext>(options => options.UseSqlServer(configuration["SqlConnStr"]));

builder.Services.AddScoped<ICatalogRepo, SqlCatalogRepo>();

builder.Services.AddMassTransit(x => {
    x.UsingRabbitMq();
});

builder.Services.AddOptions<MassTransitHostOptions>()
    .Configure(opt => {
        opt.WaitUntilStarted = true;
        opt.StartTimeout = TimeSpan.FromSeconds(10);
        opt.StopTimeout = TimeSpan.FromSeconds(30);
    });

Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", "Catalog.Api")
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

builder.Host.UseSerilog(Log.Logger);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

try {
    Console.WriteLine("Applying migrations");
    PrepDB.MigrateDatabase(app);
}
catch(Exception ex) {
    Console.WriteLine($"Program terminated unexpectedly: {ex.Message}");
}

if (app.Environment.IsDevelopment()) {   
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

try {
    Log.Information("Application is starting");
    app.Run();
}
catch(Exception ex) {
    Log.Fatal(ex, "Application failed to start");
}
finally
{
    Log.CloseAndFlush();
}

