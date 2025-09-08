using DartClub.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartClub.Application.Abstractions
{
    /// <summary>
    /// Abstraktion über die Datenzugriffschicht.
    /// Die Web-Schicht kennt nur dieses Interface, nicht EF-Core
    /// </summary>
    public interface IMemberService
    {
        // Liefert alle Mitglieder zurück (read-only).
        Task<IReadOnlyList<Member>> GetAllAsync(CancellationToken ct = default);
    }
}
