using FiapCloudGames.Application;
using FiapCloudGames.Application.Commands.RegisterUser;
using FiapCloudGames.Infrastructure;
using NetDevPack.SimpleMediator;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddApplicationModule();
builder.Services.AddInfrastructureModule(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddSimpleMediator();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
