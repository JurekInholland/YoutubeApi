using System.Reflection;
using System.Text.Json.Serialization;
using App.Middleware;
using Domain;
using Domain.Context;
using Domain.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Models;
using Services;
using Services.DownloadService;
using Services.QueueService;
using Services.ScrapeService;
using Services.TaskService;
using Services.ThumbnailService;
using Services.Validators;
using Services.YoutubeApiService;
using Services.YoutubeExplodeService;
using Services.YoutubeService;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<RouteOptions>(options => options.LowercaseQueryStrings = true);
// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo {Title = "youtube.juri.lol", Version = "v1"});
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.Configure<AppConfig>(cfg =>
{
    cfg.DataPath = builder.Configuration.GetValue<string>("DataPath") ?? string.Empty;
    cfg.YtDlPath = builder.Configuration.GetValue<string>("YtDlPath") ?? string.Empty;
    cfg.YoutubeApiKey = builder.Configuration.GetValue<string>("YoutubeApiKey") ?? string.Empty;
});

builder.Services.AddDbContext<YoutubeAppContext>(options => { options.UseSqlite("Data Source=data/youtube.db"); });

builder.Services.AddSignalR();
builder.Services.AddSingleton<YoutubeHub>();


builder.Services.AddScoped<IYoutubeService, YoutubeService>();
builder.Services.AddScoped<IYoutubeApiService, YoutubeApiService>();
builder.Services.AddScoped<IDownloadService, DownloadService>();
builder.Services.AddScoped<IYoutubeExplodeService, YoutubeExplodeService>();
builder.Services.AddScoped<IThumbnailService, ThumbnailService>();
builder.Services.AddScoped<IScrapeService, ScrapeService>();
// builder.Services.AddScoped<IQueueService, QueueService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// builder.Services.AddHostedService<TaskService>();

// builder.Services.AddSingleton<TaskService>();
// builder.Services.AddHostedService<TaskService>();
// builder.Services.AddHostedService<QueueService>();
builder.Services.AddTransient<IQueueService, QueueService>();
builder.Services.AddTransient<ITaskService, TaskService>();
// builder.Services.AddHostedService<TaskService>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<ITaskService>());

builder.Services.AddHttpClient();
builder.Services.AddSignalR();

builder.Services.AddSingleton<YoutubeHub>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// builder.Services.AddFluentValidation();
builder.Services.AddValidatorsFromAssemblyContaining<DownloadYoutubeVideoValidator>(); // register validators

WebApplication app = builder.Build();

app.UseMiddleware<RequestDurationMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();
app.UseStaticFiles();

app.MapControllers();
app.MapHub<YoutubeHub>("/api/signalr");

// app.UseMiddleware<ProxyMiddleware>();
await app.RunAsync();
