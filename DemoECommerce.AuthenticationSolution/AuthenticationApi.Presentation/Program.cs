using AuthenticationApi.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Ajouter les services n�cessaires
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureService(builder.Configuration);

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

var app = builder.Build();

// Activer la politique CORS
app.UseCors();

// Configuration de l'infrastructure et autres middlewares
app.UserInfrastructurePolicy();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
