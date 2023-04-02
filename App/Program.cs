using System.Reflection;
using App;
using Microsoft.OpenApi.Models;
using Models;
using Services.DownloadService;
using Services.YoutubeApiService;
using Services.YoutubeExploseService;
using Services.YoutubeService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RouteOptions>(options => options.LowercaseQueryStrings = true);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo {Title = "youtube.juri.lol", Version = "v1"});
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
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

builder.Services.AddTransient<IYoutubeService, YoutubeService>();
builder.Services.AddTransient<IYoutubeApiService, YoutubeApiService>();
builder.Services.AddTransient<IDownloadService, DownloadService>();
builder.Services.AddTransient<IYoutubeExplodeService, YoutubeExplodeService>();

builder.Services.AddHttpClient();
builder.Services.AddSignalR();

builder.Services.AddSingleton<YoutubeHub>();


var app = builder.Build();


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

// app.UseMiddleware<ProxyMiddleware>();
await app.RunAsync();
