using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using RailcarTrips.Components;
using RailcarTrips.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers();  // Enables API controllers

// if there's a need for httpclient
// // Register HttpClient for Blazor WebAssembly
// builder.Services.AddScoped(sp => new HttpClient
// {
//     BaseAddress = new Uri(builder.Configuration["ApiUrl"] ?? "https://localhost:7186") 
// });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("https://railcartripsclient.azurewebsites.net/")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Retrieve Azure Key Vault URI from appsettings.json
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

app.UseCors("AllowBlazorClient"); 
app.MapControllers(); // registers API controllers

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(RailcarTrips.Client._Imports).Assembly);

app.Run();