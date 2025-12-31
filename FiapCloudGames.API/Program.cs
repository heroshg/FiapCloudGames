using FiapCloudGames.Application;
using FiapCloudGames.Infrastructure;
using NetDevPack.SimpleMediator;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationModule();
builder.Services.AddInfrastructureModule(builder.Configuration);

// SimpleMediator (override IMediator lifetime to Scoped)
builder.Services.AddSimpleMediator();
builder.Services.AddScoped<IMediator, Mediator>();

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
