using System;
using System.Collections.Generic;
using System.ComponentModel;
using Platero.Nina.Core.Extensions;

namespace Platero.Nina.Core.Abstractions.Models
{
    /// <summary>
    /// Entspricht dem amtlichen Regionalschlüssel (ARS) des statistischen Bundesamtes.
    /// </summary>
    public class AreaCode
    {
        /// <summary>
        /// Erstellt einen neuen Regionalcode für ein Gebiet.
        /// </summary>
        /// <param name="key"> Der amtliche Regionalschlüssel des statistischen Bundesamtes. </param>
        /// <param name="name"> Die Bezeichnung der Region. </param>
        /// <param name="comment"> Der Hinweis für die Region. </param>
        public AreaCode(string key, string name, string? comment = null) => (Key, Name, Comment) = (key, name, comment);

        /// <summary>
        /// Ruft den amtlichen Regionalschlüssel ab.
        /// </summary>
        public string Key { get; }
        
        /// <summary>
        /// Ruft die Bezeichnung des Gebiets ab. 
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// Ruft den Hinweis zum Gebiet ab.
        /// </summary>
        public string? Comment { get; set; }

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is AreaCode areaCode && areaCode.Key.Equals(Key, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        public override int GetHashCode() => Key.GetHashCode();
    }

    
    public class NinaDashboard
    {
        public HashSet<NinaMessage> Messages { get; set; }
    }

    /// <summary>
    /// Eine Nachricht (Warnung) des Bundesamtes für Bevölkerungsschutz.
    /// </summary>
    public class NinaMessage
    {
        /// <summary>
        /// Erstellt eine neue NINA-Nachricht.
        /// </summary>
        /// <param name="type">Der Typ der Nachricht.</param>
        /// <param name="id">Die ID der Nachricht (vergeben vom Herausgeber).</param>
        /// <param name="hash">Der Hash der Nachricht (vergeben vom Herausgeber).</param>
        public NinaMessage(NinaMessageType type, string id, string hash) => (Id, Hash, Type) = (id, hash, type);
        
        /// <summary>
        /// Ruft die vom Herausgeber der Nachricht vergebene ID ab.
        /// </summary>
        public string Id { get; }
        
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

    public class NinaMessageType
    {
        public NinaMessageType(NinaMessageContentType content, NinaMessageStatusType status,
            NinaSeverityLevel severity) => (Content, Status, Severity) = (content, status, severity);
        
        public NinaMessageContentType Content { get; }
        public NinaMessageStatusType Status { get; }
        public NinaSeverityLevel Severity { get; }

        /// <inheritdoc />
        public override string ToString() => $"{Status.ToDescription()} / {Content.ToDescription()} / {Severity.ToDescription()}";
    }
    
    public enum NinaMessageContentType
    {
        [Description("Alarm")]
        Alert = 64
    }

    public enum NinaSeverityLevel
    {
        [Description("geringfügig")]
        Minor,
        [Description("schwerwiegend")]
        Severe
    }

    public enum NinaMessageStatusType
    {
        [Description("Aktualisierung")]
        Update = 1,
    }
}