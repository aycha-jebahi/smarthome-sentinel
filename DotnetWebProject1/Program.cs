using DotnetWebProject1.Data;
using DotnetWebProject1.Services;
using Microsoft.EntityFrameworkCore;
using DotnetWebProject1.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews(); // remplace AddControllers() seul : gère API + Vues MVC
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<AnomalyDetectionService>();
builder.Services.AddHostedService<TelemetrySimulatorService>();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();     // sert les fichiers wwwroot (JS, CSS)
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.MapHub<AnomalyHub>("/hubs/anomaly");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // pour le dashboard MVC

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbInitializer.Seed(context);
}

app.Run();