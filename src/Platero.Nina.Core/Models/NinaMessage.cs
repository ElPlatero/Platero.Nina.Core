using System;

namespace Platero.Nina.Core.Models
{
    /// <summary>
    /// Eine Nachricht (Warnung) des Bundesamtes für Bevölkerungsschutz.
    /// </summary>
    public class NinaMessage : WarnMessageBase
    {
        /// <summary>
        /// Erstellt eine neue NINA-Nachricht.
        /// </summary>
        /// <param name="type">Der Typ der Nachricht.</param>
        /// <param name="id">Die ID der Nachricht (vergeben vom Herausgeber).</param>
        /// <param name="hash">Der Hash der Nachricht (vergeben vom Herausgeber).</param>
        public NinaMessage(NinaMessageType type, string id, string hash): base(id) => (Hash, Type) = (hash, type);
        
        /// <summary>
        /// Ruft den vom Herausgeber der Nachricht vergebenen Hash ab.
        /// </summary>
        public string Hash { get; }
        
        /// <summary>
        /// Ruft das Datum ab, zu dem die Warnung gesendet wurde.
        /// </summary>
        public DateTime Date { get; init; } = DateTime.MinValue;
        
        /// <summary>
        /// Ruft den Typ der Nachricht ab.
        /// </summary>
        public NinaMessageType Type { get; }

        /// <summary>
        /// Ruft den Titel der Nachricht ab.
        /// </summary>
        public string Title { get; init; } = string.Empty;
        
        /// <summary>
        /// Ruft die Quelle der Nachricht ab.
        /// </summary>
        public string Source { get; init; } = string.Empty;

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is NinaMessage other && Id == other.Id && Hash == other.Hash;

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(Id, Hash);
    }
}