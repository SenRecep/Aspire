using CompanyName.BuildingBlocks.OpenTelemetry.Base;
using CompanyName.BuildingBlocks.Presentation.HealthChecks;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddServiceDiscovery();
builder.Services.AddHealthChecks();
builder.Services
          .ConfigureOpenTelemetry(builder.Environment.ApplicationName)
          .AddOtlpExporter();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver();

builder.AddSeqEndpoint("seq");


WebApplication app = builder.Build();

app.MapReverseProxy();

app.UseDefaultHealthChecks();

await app.RunAsync();
