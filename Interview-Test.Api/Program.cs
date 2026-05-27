using Interview_Test.Infrastructure;
using Interview_Test.Middlewares;
using Interview_Test.Repositories;
using Interview_Test.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddMvc(options => options.EnableEndpointRouting = false);
builder.Services.Configure<Microsoft.AspNetCore.Mvc.ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// connection string
var connection = builder.Configuration.GetConnectionString("DefaultConnection")
                 ?? "Server=localhost,1433;Database=InterviewTestDb;User=sa;Password=@Passw0rd;TrustServerCertificate=True;MultipleActiveResultSets=true";

// DbContext + retry
builder.Services.AddDbContext<InterviewTestDbContext>(options =>
{
    options.UseSqlServer(connection, sqlOptions =>
    {
        sqlOptions.UseCompatibilityLevel(110);
        sqlOptions.CommandTimeout(30);
        sqlOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(10), errorNumbersToAdd: null);
    });
});

// DI
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddTransient<AuthenMiddleware>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

// Swagger (dev only)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// auto migrate + seed (retry รอ SQL Server พร้อม)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<InterviewTestDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var attempts = 0;
    const int maxAttempts = 20;
    while (true)
    {
        try
        {
            db.Database.Migrate();
            break;
        }
        catch (Exception ex) when (attempts < maxAttempts)
        {
            attempts++;
            logger.LogWarning("Migration attempt {Attempt}/{Max} failed: {Message}. Retrying in 5s...",
                attempts, maxAttempts, ex.Message);
            Thread.Sleep(5000);
        }
    }
    DbInitializer.Seed(db);
}

// pipeline: CORS → ตรวจ api key → routing
app.UseCors();
app.UseMiddleware<AuthenMiddleware>();
app.UseMvc();
app.Run();
