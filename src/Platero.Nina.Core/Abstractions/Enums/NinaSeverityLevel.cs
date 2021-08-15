using System.ComponentModel;

namespace Platero.Nina.Core.Abstractions.Enums
{
    /// <summary>
    /// Mögliche Schweregrade des Inhalts einer Meldung.
    /// </summary>
    public enum NinaSeverityLevel
    {
        /// <summary>
        /// Warnung vor einem geringfügigen Vorfall.
        /// </summary>
        [Description("geringfügig")]
        Minor,
        /// <summary>
        /// Warnung vor einem schweren Vorfall.
        /// </summary>
        [Description("schwerwiegend")]
        Severe
    }
}