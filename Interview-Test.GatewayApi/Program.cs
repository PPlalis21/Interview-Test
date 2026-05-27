using Ocelot.DependencyInjection;
using Ocelot.Middleware;

// API Gateway (Ocelot) — forward /gateway/* → backend API
var builder = WebApplication.CreateBuilder(args);

// โหลด route config
builder.Configuration.AddJsonFile("configurationOcelot.json", optional: false, reloadOnChange: true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOcelot(builder.Configuration);

// CORS
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

app.UseCors();
app.UseHttpsRedirection();
await app.UseOcelot();
app.Run();
