var builder = DistributedApplication.CreateBuilder(args);


var cache = builder.AddRedisContainer("aspire-cache");

builder.AddProject<Projects.Aspire_Test_Calendar_Api>("aspire.test.calendar.api")
    .WithReference(cache);

builder.Build().Run();
