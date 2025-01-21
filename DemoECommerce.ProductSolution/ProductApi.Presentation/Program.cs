using Application.Services.ServicesImpl;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using ProductApi.Domain.Interfaces;
using ProductApi.Infrastructure;
using Serilog;
using ProductApi.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Lire la configuration
var configuration = builder.Configuration;

// Configurer Serilog pour la journalisation
var logFileName = configuration["MySerilog:FileName"] ?? "app"; // Nom de fichier par d�faut si non sp�cifi�
Log.Logger = new LoggerConfiguration()
    .WriteTo.File($"{logFileName}-.log", rollingInterval: RollingInterval.Day, fileSizeLimitBytes: 50_000_000) // Limite de taille des fichiers de log (50MB)
    .WriteTo.Console()  // Afficher les logs dans la console (utile pour le d�veloppement)
    .WriteTo.Debug()    // Afficher les logs dans la fen�tre de d�bogage (utile pour le d�veloppement)
    .CreateLogger();

// Ajouter Serilog comme fournisseur de journalisation pour l'application
builder.Host.UseSerilog();

// Configurer le DbContext pour l'acc�s � la base de donn�es
builder.Services.AddDbContext<MarketDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("eCommerceConnection"))
           .EnableSensitiveDataLogging() // Active la journalisation des donn�es sensibles (� utiliser uniquement en d�veloppement)
           .LogTo(Console.WriteLine));   // Affiche les requ�tes SQL dans la console (utile pour le d�veloppement)

// Ajouter les services de votre application
builder.Services.AddScoped<IMarketService, MarketService>();
builder.Services.AddScoped<IMarketRepository, MarketRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Ajouter la configuration des CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin() // Permet les requ�tes de n'importe quelle origine
              .AllowAnyHeader() // Permet tous les en-t�tes
              .AllowAnyMethod(); // Permet toutes les m�thodes HTTP (GET, POST, etc.)
    });
});

// Ajouter les services pour les contr�leurs, la documentation Swagger, etc.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Activer Swagger pour la documentation de l'API
app.UseSwagger();
app.UseSwaggerUI();

// Activer la politique CORS
app.UseCors();

// Configurer le middleware pour l'API
app.UseHttpsRedirection();  // Pour forcer le protocole HTTPS
app.UseAuthorization();      // Pour activer l'autorisation (si vous en avez besoin)
app.MapControllers();        // Mapper les contr�leurs (routes API)

// Lancer l'application
app.Run();
