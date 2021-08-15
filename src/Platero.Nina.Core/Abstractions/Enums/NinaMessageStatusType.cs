using System.ComponentModel;

namespace Platero.Nina.Core.Abstractions.Enums
{
    /// <summary>
    /// Mögliche Ausprägungen des Status einer NINA-Meldung.
    /// </summary>
    public enum NinaMessageStatusType
    {
        /// <summary>
        /// Eine Aktualisierung.
        /// </summary>
        [Description("Aktualisierung")]
        Update = 1
    }
}