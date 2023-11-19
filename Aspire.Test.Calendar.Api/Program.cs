using System.Text.Json;
using Aspire.Test.Calendar.Api.CalendarService;
using Refit;
using System.Text.Json.Serialization;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddRedis("aspire-cache");


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRefitClient<ICalendarApiService>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://holidayapi.ir"));


var app = builder.Build();

app.MapDefaultEndpoints();

app.MapGet("/api/events/{year}/{month}/{day}", async (ICalendarApiService apiService
    , IConnectionMultiplexer redisConnection
    ,ILogger<Program> logger
    , int year,int month,int day) =>
{
    var cacheKey = $"georgian_{year}_{month}_{day}";

   var database= redisConnection.GetDatabase(0);

   if (await database.KeyExistsAsync(cacheKey))
   {
        logger.LogWarning("Cache Exists! Key: {cacheKey}",cacheKey);
        var cachedValue = JsonSerializer.Deserialize<List<EventApiModel>?>(await database.StringGetAsync(cacheKey));

        return Results.Ok(cachedValue);
   }

    var apiResponse = await apiService.GetGeorgianDate(year, month, day);

    var events = apiResponse.Events
        .Select(c => new EventApiModel(c.Description, c.AdditionalDescription, c.IsHoliday, c.IsReligious))
        .ToList();

    await database.StringSetAsync(cacheKey, JsonSerializer.Serialize(events));
    
    return Results.Ok(events);
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();



app.Run();


public record EventApiModel(
   string Description,
     string AdditionalDescription,
     bool IsHoliday,
    bool IsReligious
);