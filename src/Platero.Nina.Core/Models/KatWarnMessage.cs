using System;
using Platero.Nina.Core.Abstractions.Enums;

namespace Platero.Nina.Core.Models
{
    /// <summary>
    /// Bildet eine Katastrophen-Warnmeldung ab.
    /// </summary>
    public class KatWarnMessage
    {
        /// <summary>
        /// Erzeugt eine neue Instanz einer Katastrophen-Warnmeldung.
        /// </summary>
        /// <param name="id">Der eindeutige Bezeichner der Meldung.</param>
        public KatWarnMessage(string id) => Id = id;
        
        /// <summary>
        /// Der eindeutige Bezeichner der Meldung.
        /// </summary>
        public string Id { get; }
        
        /// <summary> Die Version der Meldung. </summary>
        public int Version { get; init; }
        
        /// <summary>
        /// Der Beginn der Gültigkeit der Meldung.
        /// </summary>
        public DateTime StartDate { get; init; }
        
        /// <summary>
        /// Der Schweregrad des Inhalts der Meldung.
        /// </summary>
        public NinaSeverityLevel Severity { get; init; }
        
        /// <summary>
        /// Die Art der Meldung.
        /// </summary>
        public NinaMessageContentType ContentType { get; init; }
        
        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is KatWarnMessage other && Id.Equals(other.Id, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        public override int GetHashCode() => Id.GetHashCode();
    }
}