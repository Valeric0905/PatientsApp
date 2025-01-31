using Microsoft.IdentityModel.Tokens;
using System.Text;
using PatientsApp.Data;
using System;
using PatientsApp.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity services
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Add custom repositories
builder.Services.AddScoped<IPatientRepository, PatientRepository>();

// JWT Authentication Configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("HealthcarePolicy", policy => policy.RequireRole("HealthcareProfessional"));
});

// Add API Documentation (Swagger)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Controllers
builder.Services.AddControllers();

// CORS Configuration for Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // Angular app URL
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Build the application
var app = builder.Build();

// Middleware Configuration
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = "swagger";
    });
}

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    c.RoutePrefix = string.Empty;
});

app.UseCors("AllowAngularApp");
app.UseHttpsRedirection();
app.UseAuthentication(); // Enable Authentication Middleware
app.UseAuthorization();  // Enable Authorization Middleware

// Map Controllers (API routes)
app.MapControllers();

// Set HTTP/2 support flag
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

// Run the application
app.Run();
