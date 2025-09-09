using DartClub.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartClub.Infrastructure.Persistence
{
    /// <summary>
    /// Kleiner Seeder für Entwicklungszwecke.
    /// - Läuft bei App-Start im Development.
    /// - Fügt Beispiel-Mitglieder ein, falls die Tabelle leer ist.
    /// </summary>
    public class DbInitializer
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if(await context.Members.AnyAsync())
            {
                return; // Tabelle hat bereits Daten
            }

            // Beispiel-Datensätze (nur für DEV)
            var members = new[]
            {
                new Member{ Id = Guid.NewGuid(), Name = "Max Mustermann", Email = "max.mustermann@gmail.com"},
                new Member{ Id = Guid.NewGuid(), Name = "Erika Musterfrau", Email = "erika@gmail.com" }
            };

            // Datensätze anhängen und speichern
            await context.Members.AddRangeAsync(members);
            await context.SaveChangesAsync();
        }
    }
}
