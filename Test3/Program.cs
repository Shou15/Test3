using MongoDB.Driver;
using Test3.Components;
using Test3.Data.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// MongoDB client
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var connectionString = builder.Configuration["MongoSettings:ConnectionString"];
    return new MongoClient(connectionString);
});

// Database
builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    var dbName = builder.Configuration["MongoSettings:DatabaseName"];
    return client.GetDatabase(dbName);
});

// Register UserService
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ApartmentService>();
builder.Services.AddScoped<RoomService>();

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
