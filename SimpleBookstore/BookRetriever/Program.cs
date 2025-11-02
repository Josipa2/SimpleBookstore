using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleBookstore.Domain;
using SimpleBookstore.Domain.Interfaces.Repositories;
using SimpleBookstore.Domain.Interfaces.Services;
using SimpleBookstore.Domain.Repositories;
using SimpleBookstore.Domain.Services;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
 .AddApplicationInsightsTelemetryWorkerService()
 .ConfigureFunctionsApplicationInsights()
 .AddTransient<IBookService, BookService>()
 .AddTransient<IBookRepository, BookRepository>();

var connectionString = builder.Configuration["ConnectionStrings:SimpleBookstoreDbConnection"];
builder.Services.AddDbContext<SimpleBookstoreDbContext>(options =>
 options.UseNpgsql(connectionString));

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
 var services = scope.ServiceProvider;

 var context = services.GetRequiredService<SimpleBookstoreDbContext>();
 context.Database.Migrate();
}

host.Run();
