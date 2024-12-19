using CompanyName.BuildingBlocks.Database.Migrator;
using CompanyName.Services.Info;
using CompanyName.Services.ProductService.Persistence.EntityFrameworkCore.Contexts;
using CompanyName.Services.ProductService.WebApi.ServiceRegistrations;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddServiceRegistrations(builder.Environment, builder.Configuration);
builder.AddSeqEndpoint(ServiceKeys.Seq);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
    await app.MigrateAsync<ApplicationWriteDbContext>();

app.UseServices();

await app.RunAsync();
