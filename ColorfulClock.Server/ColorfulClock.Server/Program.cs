using ColorfulClock.Server.Services;
using ColorfulClock.Server.Services.Contracts;
using ColorfulClock.Server.Hubs;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLogging();

builder.Services.AddSignalR()
    .AddNewtonsoftJsonProtocol(bla => bla.PayloadSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

builder.Services.AddHostedService<TimeBackgroundService>();
builder.Services.AddHostedService<ColorBackgroundService>();

builder.Services.AddSingleton<IColoredTimeService, ColoredTimeService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:8080", "http://192.168.0.148:8080")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ColoredClockHub>("/coloredClockHub");
});

app.Run();
