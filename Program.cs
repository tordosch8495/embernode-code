using Microsoft.Azure.Cosmos;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using EmbernodeApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Azure AD Authentifizierung hinzufügen
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

// Autorisierung aktivieren
builder.Services.AddAuthorization();

// Razor Pages hinzufügen
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

// Konfiguration der HTTP-Anfrage-Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Authentifizierung und Autorisierung aktivieren
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();