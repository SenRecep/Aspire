using CompanyName.BuildingBlocks.Database.Migrator;
using CompanyName.Services.CategoryService.Persistence.EntityFrameworkCore.Contexts;
using CompanyName.Services.CategoryService.WebApi.ServiceRegistrations;
// using CompanyName.Services.Info;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddServiceRegistrations(builder.Environment, builder.Configuration);
// builder.AddSeqEndpoint(ServiceKeys.Seq);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
    await app.MigrateAsync<ApplicationWriteDbContext>();

app.UseServices();

await app.RunAsync();
