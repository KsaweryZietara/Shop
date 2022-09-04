using Basket.Api.Data;
using Basket.Api.IntegrationEvents.EventHandling;
using MassTransit;
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

app.Run();
