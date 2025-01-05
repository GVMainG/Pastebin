using MongoDB.Driver;
using Quartz;
using StackExchange.Redis;
using TextService.Services.Interfaces;
using TextService.Services;
using TextService.Repositorys;
using TSS = TextService.Services.TextService;

var builder = WebApplication.CreateBuilder(args);

// ��������� MongoDB
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
    new MongoClient("mongodb://localhost:27017"));
builder.Services.AddSingleton<IMongoDatabase>(sp =>
    sp.GetRequiredService<IMongoClient>().GetDatabase("pastebin"));

// Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost"));

// ����������� ��������
builder.Services.AddScoped<ITextService, TSS>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<TextRepository>();

builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();
app.Run();