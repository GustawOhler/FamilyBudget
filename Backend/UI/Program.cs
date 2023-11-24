using System.Text;
using Application.Auth;
using FamilyBudget;
using FamilyBudgetApplication.Auth;
using FamilyBudgetDomain.Interfaces;
using FamilyBudgetDomain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

const string MY_CORS_POLICY = "_MyCorsPolicy";

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MY_CORS_POLICY,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000",
                                             "https://localhost:3000",
                                             "http://127.0.0.1:3000",
                                             "https://127.0.0.1:3000").AllowAnyMethod().AllowAnyHeader();
                      });
});

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddDbContext<FamilyBudgetDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("FamilyBudgetDatabase")));

builder.Services.AddIdentityCore<User>()
        .AddEntityFrameworkStores<FamilyBudgetDbContext>()
        .AddDefaultTokenProviders();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = config["JWT:ValidIssuer"],
        ValidAudience = config["JWT:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ITokenGenerator, JwtTokenGenerator>();
builder.Services.AddTransient<IAuthenticator, SimpleAuthenticator>();
builder.Services.AddTransient<IUserRegistrant, UserRegistrant>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MY_CORS_POLICY);
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
