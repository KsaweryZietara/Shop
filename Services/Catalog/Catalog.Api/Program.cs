using Catalog.Api.Data;
using Catalog.Api.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddDbContext<CatalogContext>(options => options.UseSqlServer(configuration["SqlConnStr"]));

builder.Services.AddScoped<ICatalogRepo, SqlCatalogRepo>();

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

app.Run();
