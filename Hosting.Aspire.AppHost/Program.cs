var builder = DistributedApplication.CreateBuilder(args);


var cache = builder.AddRedisContainer("aspire-cache");

var api=builder.AddProject<Projects.Aspire_Test_Calendar_Api>("aspire.test.calendar.api")
    .WithReference(cache);

builder.AddProject<Projects.Aspire_Test_Blazor_UI>("aspire.test.blazor.ui")
    .WithReference(api);

builder.Build().Run();
