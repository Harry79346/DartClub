using Microsoft.EntityFrameworkCore;
using DartClub.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartClub.Infrastructure.Persistence
{
    /// <summary>
    /// Der zentrale Einstiegspunkt für EF Core.
    /// - Mappt .NET-Klassen (Entities) auf DB-Tabellen.
    /// - Bekommt seine Optionen (z. B. Connection String) per DI.
    /// Hinweis: Dieser Typ liegt bewusst in "Infrastructure", damit Domain sauber bleibt.
    /// </summary>
    public sealed class AppDbContext : DbContext
    {
        /// <summary>
        /// EF Core ruft diesen Konstruktor auf und liefert dabei die DB-Optionen (Provider, Connection String, etc.).
        /// Diese Optionen registrieren wir später in Program.cs (Web) mit AddDbContext(...).
        /// </summary>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>
        /// Repräsentiert die Tabelle "Members".
        /// - Der Name entspricht per Konvention dem DbSet-Namen (Plural wird i. d. R. übernommen).
        /// - EF Core nutzt die "Member"-Klasse aus der Domain als Zeilen-Modell.
        /// </summary>
        public DbSet<Member> Members => Set<Member>();

        /// <summary>
        /// Hier definieren wir DB-spezifische Regeln (Constraints, Indizes, Längen).
        /// Das hält unsere Domain-Klassen schlicht und verschiebt DB-Details hierher.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfiguration für die Member-Entity
            modelBuilder.Entity<Member>(entity =>
            {
                // Primärschlüssel
                entity.HasKey(m => m.Id);

                // Name ist Pflichtfeld, max. 200 Zeichen (wird als NVARCHAR(200) abgebildet)
                entity.Property(m => m.Name)
                      .IsRequired()
                      .HasMaxLength(200);

                // Email ist Pflichtfeld, max. 320 Zeichen (gängig für E-Mail-Längen)
                entity.Property(m => m.Email)
                      .IsRequired()
                      .HasMaxLength(320);

                // E-Mail soll einzigartig sein → Unique Index
                entity.HasIndex(m => m.Email)
                      .IsUnique();
            });

            // WICHTIG: Immer base.OnModelCreating(...) aufrufen? Hier nicht zwingend,
            // weil wir keine Basisklasse mit eigenem Verhalten haben. Bei Bedarf:
            // base.OnModelCreating(modelBuilder);
        }
    }
}
