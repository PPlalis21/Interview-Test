using Interview_Test.Infrastructure;
using Interview_Test.Middlewares;
using Interview_Test.Services;
using Interview_Test.UnitOfWork;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddMvc(options => options.EnableEndpointRouting = false);
builder.Services.Configure<Microsoft.AspNetCore.Mvc.ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var connection = builder.Configuration.GetConnectionString("DefaultConnection")
                 ?? "Server=localhost,1433;Database=InterviewTestDb;User=sa;Password=@Passw0rd;TrustServerCertificate=True;MultipleActiveResultSets=true";

builder.Services.AddDbContext<InterviewTestDbContext>(options =>
{
    options.UseSqlServer(connection, sqlOptions =>
    {
        sqlOptions.UseCompatibilityLevel(110);
        sqlOptions.CommandTimeout(30);
        sqlOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(10), errorNumbersToAdd: null);
    });
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddTransient<AuthenMiddleware>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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

app.UseCors();
app.UseMiddleware<AuthenMiddleware>();
app.UseMvc();
app.Run();
