using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using RailcarTrips.Client.Pages;
using RailcarTrips.Components;
using RailcarTrips.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

// Retrieve Azure Key Vault URI from environment variable or appsettings.json
var keyVaultUri = builder.Configuration["VaultUri"];

if (string.IsNullOrEmpty(keyVaultUri))
{
    throw new InvalidOperationException("Vault URI is not set.");
}

// Create a SecretClient to access secrets in Key Vault
var client = new SecretClient(vaultUri: new Uri(keyVaultUri), credential: new DefaultAzureCredential());

// Retrieve connection strings from Key Vault
var defaultConnectionSecret = client.GetSecret("DefaultConnection");
var databaseConnectionSecret = client.GetSecret("DatabaseConnection");

// Use the connection strings from Key Vault in your DbContext
builder.Services.AddDbContext<RailcarTripsContext>(options =>
{
    //options.UseSqlServer(defaultConnectionSecret.Value.Value); // For LocalDB or Default Connection
    options.UseSqlServer(databaseConnectionSecret.Value.Value); // For Azure SQL Server Connection
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(RailcarTrips.Client._Imports).Assembly);

app.Run();