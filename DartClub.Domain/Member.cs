using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartClub.Domain
{
    /// <summary>
    /// Repräsentiert ein Vereinsmitglied.
    /// EF Core wird diese Klasse als Tabelle abbilden.
    /// </summary>
    public class Member
    {
        // Primary Key - Guid ist praktisch in verteilten Systemen.
        public Guid Id { get; set; }
        
        // Name des Mitglieds. (Später in EF: Required + MaxLength)
        public string Name { get; set; } = string.Empty;
        
        // E-Mail des Mitglieds. (Später: Unique Index)
        public string Email { get; set; } = string.Empty;
    }
}
