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
var logFileName = configuration["MySerilog:FileName"] ?? "app"; // Nom de fichier par défaut si non spécifié
Log.Logger = new LoggerConfiguration()
    .WriteTo.File($"{logFileName}-.log", rollingInterval: RollingInterval.Day, fileSizeLimitBytes: 50_000_000) // Limite de taille des fichiers de log (50MB)
    .WriteTo.Console()  // Afficher les logs dans la console (utile pour le développement)
    .WriteTo.Debug()    // Afficher les logs dans la fenêtre de débogage (utile pour le développement)
    .CreateLogger();

// Ajouter Serilog comme fournisseur de journalisation pour l'application
builder.Host.UseSerilog();

// Configurer le DbContext pour l'accès à la base de données
builder.Services.AddDbContext<MarketDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("eCommerceConnection"))
           .EnableSensitiveDataLogging() // Active la journalisation des données sensibles (à utiliser uniquement en développement)
           .LogTo(Console.WriteLine));   // Affiche les requêtes SQL dans la console (utile pour le développement)

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
        policy.AllowAnyOrigin() // Permet les requêtes de n'importe quelle origine
              .AllowAnyHeader() // Permet tous les en-têtes
              .AllowAnyMethod(); // Permet toutes les méthodes HTTP (GET, POST, etc.)
    });
});

// Ajouter les services pour les contrôleurs, la documentation Swagger, etc.
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
app.MapControllers();        // Mapper les contrôleurs (routes API)

// Lancer l'application
app.Run();
