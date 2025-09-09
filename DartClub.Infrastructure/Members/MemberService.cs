using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DartClub.Domain;
using DartClub.Application.Abstractions;
using DartClub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DartClub.Infrastructure.Members
{
    /// <summary>
    /// Konkrete Implementierung in IMemberService mit EF Core.
    /// - Liest Mitglieder aus der DB (read-only).
    /// - Liegt in der Infrastructure, damit die Web/Application-Schicht
    ///   keine EF-Core-Abhängigkeit hat.
    /// </summary>
    public sealed class MemberService : IMemberService
    {
        private readonly AppDbContext _context;

        public MemberService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Liefert alle Mitglieder alphabeitsch sortiert zurück.
        /// WICHTIG: AsNoTracking() für reine Leseabfragen -> schneller, weniger Speicher.
        /// </summary>
        public async Task<IReadOnlyList<Member>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Members
                .AsNoTracking()
                .OrderBy(m => m.Name)
                .ToListAsync(ct);
        }
    }
}
