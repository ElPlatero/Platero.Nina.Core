using System.ComponentModel;

namespace Platero.Nina.Core.Abstractions.Enums
{
    /// <summary>
    /// Mögliche Arten des Inhalts einer NINA-Meldung.
    /// </summary>
    public enum NinaMessageContentType
    {
        /// <summary>
        /// Ein Alarm.
        /// </summary>
        [Description("Alarm")]
        Alert
    }
}