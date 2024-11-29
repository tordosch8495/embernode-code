using Microsoft.Azure.Cosmos;
using EmbernodeApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Logging konfigurieren
builder.Logging.ClearProviders();               // Entfernt alle Standard-Logger
builder.Logging.AddConsole();                   // Konsolen-Logging (Debugging lokal)
builder.Logging.AddAzureWebAppDiagnostics();    // Aktiviert Logging für Azure App Service

// Add services to the container.
builder.Services.AddRazorPages();

// Cosmos DB Verbindung einrichten
string connectionString = builder.Configuration["CosmosDb:ConnectionString"];
builder.Services.AddSingleton<CosmosClient>(options => new CosmosClient(connectionString));
builder.Services.AddSingleton<CosmosDbService>(options =>
    new CosmosDbService(
        options.GetRequiredService<CosmosClient>(),
        "embernode",  // Datenbankname
        "testdata"    // Containername
    ));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
