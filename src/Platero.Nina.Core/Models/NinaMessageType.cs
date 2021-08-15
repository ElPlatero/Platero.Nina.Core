using Platero.Nina.Core.Abstractions.Enums;
using Platero.Nina.Core.Extensions;

namespace Platero.Nina.Core.Models
{
    /// <summary>
    /// Bildet die Art des Inhalts, den aktuellen Status und die Schwere der Meldung ab.
    /// </summary>
    public class NinaMessageType
    {
        /// <summary>
        /// Erzeugt eine neue Instanz.
        /// </summary>
        /// <param name="content">Die Art des Inhalts der Meldung.</param>
        /// <param name="status">Der Status der Meldung.</param>
        /// <param name="severity">Die Schwere des Inhalts der Meldung.</param>
        public NinaMessageType(NinaMessageContentType content, NinaMessageStatusType status,
            NinaSeverityLevel severity) => (Content, Status, Severity) = (content, status, severity);
        
        /// <summary>
        /// Ruft die Art des Inhalts der Meldung ab.
        /// </summary>
        public NinaMessageContentType Content { get; }
        
        /// <summary>
        /// Ruft den Status der Meldung ab.
        /// </summary>
        public NinaMessageStatusType Status { get; }
        
        /// <summary>
        /// Ruft die Schwere des Inhalts der Meldung ab.
        /// </summary>
        public NinaSeverityLevel Severity { get; }

        /// <inheritdoc />
        public override string ToString() => $"{Status.ToDescription()} / {Content.ToDescription()} / {Severity.ToDescription()}";
    }
}