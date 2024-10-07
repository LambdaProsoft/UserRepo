using Application.Interfaces;
using Application.Mappers.IMappers;
using Application.Mappers;
using Application.UseCases;
using Infrastructure.Command;
using Microsoft.EntityFrameworkCore;
using UserInfrastructure.Persistence;
using UserInfrastructure.Query;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration["ConnectionString"];
builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(connectionString));

//injection dependecy
builder.Services.AddScoped<IPasswordService, PasswordService>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserCommand, UserCommand>();
builder.Services.AddScoped<IUserQuery, UserQuery>();
builder.Services.AddScoped<IUserMapper, UserMapper>();



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
