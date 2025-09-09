using DartClub.Application.Abstractions;
using DartClub.Infrastructure.Members;
using DartClub.Infrastructure.Persistence;      // der AppDbContext
using Microsoft.EntityFrameworkCore;            // UseSqlServer-Extension


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Registriert EF Core mit dem SQL Server-Provider und liest den Connection String "Default".
// Ergebnis: Wenn irgendwo AppDbContext benötigt wird (z. B. im Service), liefert DI eine Instanz.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Hier sage ich dem DI-Container:
// "Wenn jemand IMemberService braucht, nimm MemberService."
builder.Services.AddScoped<IMemberService, MemberService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Dieser Block sorgt dafür, dass bei Entwicklungsstarts die DB auf dem aktuellen Stand ist
// und (falls leer) Seed-Daten bekommt.
if (app.Environment.IsDevelopment())
{
    // Ein Scope liefert uns einen kurzlebigen DI-Container
    using var scope = app.Services.CreateScope();

    // DbContext aus DI ziehen
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // 1) Schema auf neuesten Stand bringen (führt ausstehende Migrationen aus)
    //    Achtung: Für Dev bequem; in Produktion lieber Migrations explizit ausrollen.
    db.Database.Migrate();

    // 2) Seed-Daten einfügen (nur wenn Tabelle leer)
    await DbInitializer.SeedAsync(db);
}

app.Run();
