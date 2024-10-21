using Application.Interfaces;
using Application.Mappers.IMappers;
using Application.Mappers;
using Application.UseCases;
using Infrastructure.Command;
using Microsoft.EntityFrameworkCore;
using UserInfrastructure.Persistence;
using UserInfrastructure.Query;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<IAcountHttpService,AccountHttpService>(client =>
{
    client.BaseAddress = new Uri("https://localhost");
});


var connectionString = builder.Configuration["ConnectionString"];
builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(connectionString));

//jwt conf

var secretKey = builder.Configuration.GetValue<string>("Jwt:Key");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


//injection dependecy
builder.Services.AddScoped<IPasswordService, PasswordService>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserCommand, UserCommand>();
builder.Services.AddScoped<IUserQuery, UserQuery>();
builder.Services.AddScoped<IUserMapper, UserMapper>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IVerificationService, VerificationService>();
builder.Services.AddScoped<IJwtService, JwtService>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var key = config.GetValue<string>("Jwt:Key");
    var accessTokenExpirationMinutes = config.GetValue<int>("Jwt:AccessTokenExpirationMinutes");
    var refreshTokenExpirationDays = config.GetValue<int>("Jwt:RefreshTokenExpirationDays");

    return new JwtService(key, accessTokenExpirationMinutes, refreshTokenExpirationDays);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//agregado
app.UseAuthentication();
app.UseRouting();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
