using SimpleBookstore.Domain;
using Microsoft.EntityFrameworkCore;
using SimpleBookstore.Domain.Interfaces.Repositories;
using SimpleBookstore.Domain.Repositories;
using SimpleBookstore.Domain.Interfaces.Services;
using SimpleBookstore.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SimpleBookstoreDbContext>(option =>
    option.UseNpgsql(builder.Configuration.GetConnectionString("SimpleBookstoreDbConnection")));

builder.Services
    .AddScoped<IAuthorRepository, AuthorRepository>()
    .AddScoped<IAuthorService, AuthorService>()
    .AddScoped<IBookRepository, BookRepository>()
    .AddScoped<IBookService, BookService>()
    .AddScoped<IGenreRepository, GenreRepository>()
    .AddScoped<IGenreService, GenreService>()
    .AddScoped<IReviewRepository, ReviewRepository>()
    .AddScoped<IReviewService, ReviewService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<SimpleBookstoreDbContext>();
    context.Database.Migrate();
}

app.Run();