using DemoPractical.Data;
using DemoPractical.Models;
using DemoPractical.Repository;
using DemoPractical.Validations;
using DemoPractical.ViewModels;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<MovieDBContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DBCS")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<MovieDBContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://localhost:5001",
            ValidAudience = "https://localhost:5001",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345")),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddMvc();
builder.Services.AddFluentValidationAutoValidation();
builder.Host.UseSerilog((context, configuration) => 
            configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddTransient<IMovieRepository, MovieRepository>();
builder.Services.AddTransient<IMovieCastRepository, MovieCastRepository>();
builder.Services.AddScoped<IValidator<Movie>, MovieValidator>();
builder.Services.AddScoped<IValidator<RegisterViewModel>, UserValidator>();
builder.Services.AddScoped<IValidator<UpdatePasswordModel>, ProfileValidator>();



builder.Services.AddApiVersioning(v =>
{
    v.DefaultApiVersion = new ApiVersion(1, 0);
    v.AssumeDefaultVersionWhenUnspecified = true;
    v.ReportApiVersions = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
