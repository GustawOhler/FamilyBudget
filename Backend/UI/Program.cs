using System.Text;
using Application.Auth;
using FamilyBudgetUI;
using FamilyBudgetApplication.Auth;
using FamilyBudgetApplication.BalanceChangeOperations;
using FamilyBudgetApplication.BudgetOperations;
using FamilyBudgetApplication.Interfaces;
using FamilyBudgetDomain.Models;
using FamilyBudgetDomain.Validation;
using FamilyBudgetDomain.Interfaces.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Infrastructure.Database.Repositories;

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

builder.Services.AddAutoMapper(typeof(FamilyBudgetUI.MappingProfile), typeof(FamilyBudgetApplication.MappingProfile));

builder.Services.AddDbContext<FamilyBudgetDbContext>(options =>
        options.UseSqlServer(config["ConnectionStrings_FamilyBudgetDatabase"]));

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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT_Secret"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IObjectValidator, ObjectValidator>();
builder.Services.AddTransient<ITokenGenerator, JwtTokenGenerator>();
builder.Services.AddTransient<IAuthenticator, SimpleAuthenticator>();
builder.Services.AddTransient<IUserRegistrant, UserRegistrant>();
builder.Services.AddTransient<IRepository<User>, StandardRepository<User>>();
builder.Services.AddTransient<IRepository<Budget>, StandardRepository<Budget>>();
builder.Services.AddTransient<IRepository<Category>, StandardRepository<Category>>();
builder.Services.AddTransient<IRepository<BalanceChange>, StandardRepository<BalanceChange>>();
builder.Services.AddTransient<IAuthorizationVerifier, AuthorizationVerifier>();
builder.Services.AddTransient<IBudgetRetriever, BudgetRetriever>();
builder.Services.AddTransient<IBalanceChangesRetriever, BalanceChangesRetriever>();
builder.Services.AddTransient<IBalanceChangeManager, BalanceChangeManager>();
builder.Services.AddTransient<IBudgetManager, BudgetManager>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MY_CORS_POLICY);
// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
