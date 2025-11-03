using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SimpleBookstore;
using SimpleBookstore.Domain;
using SimpleBookstore.Domain.Interfaces.Repositories;
using SimpleBookstore.Domain.Interfaces.Services;
using SimpleBookstore.Domain.Repositories;
using SimpleBookstore.Domain.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwagger();

builder.Services.AddDbContext<SimpleBookstoreDbContext>(option =>
    option.UseNpgsql(builder.Configuration.GetConnectionString("SimpleBookstoreDbConnection")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["AuthSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["AuthSettings:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["AuthSettings:Token"]!)),
            ValidateIssuerSigningKey = true
        };
    });

builder.Services
    .AddScoped<IAuthorRepository, AuthorRepository>()
    .AddScoped<IAuthService, AuthService>()
    .AddScoped<IAuthorService, AuthorService>()
    .AddScoped<IBookRepository, BookRepository>()
    .AddScoped<IBookService, BookService>()
    .AddScoped<IGenreRepository, GenreRepository>()
    .AddScoped<IGenreService, GenreService>()
    .AddScoped<IReviewRepository, ReviewRepository>()
    .AddScoped<IReviewService, ReviewService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<SimpleBookstoreDbContext>();
    context.Database.Migrate();
}

app.Run();