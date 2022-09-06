using Basket.Api.Data;
using Basket.Api.EventHandling;
using MassTransit;
using Serilog;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddSingleton<IConnectionMultiplexer>(opt =>
    ConnectionMultiplexer.Connect(configuration["RedisConnStr"]));

builder.Services.AddScoped<IBasketRepo, RedisBasketRepo>();

builder.Services.AddMassTransit(x => {
    x.AddConsumer<ProductPriceChangedConsumer>(typeof(ProductPriceChangedConsumerDefinition));

    x.SetKebabCaseEndpointNameFormatter();

    x.UsingRabbitMq((context, cfg) => cfg.ConfigureEndpoints(context));
});

Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", "Basket.Api")
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

builder.Host.UseSerilog(Log.Logger);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
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














