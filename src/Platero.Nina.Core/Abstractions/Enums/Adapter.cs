namespace Platero.Nina.Core.Abstractions.Enums
{
    /// <summary>
    /// Erzeugt aus diskreten Zeichenketten entsprechende Enumerationswerte.
    /// </summary>
    public static class Adapter
    {
        /// <summary> Erzeugt den <see cref="NinaMessageContentType"/>. </summary>
        public static NinaMessageContentType? ConvertContentType(string? contentType) => contentType?.ToUpperInvariant() switch
        {
            "ALERT" => NinaMessageContentType.Alert,
            "UPDATE" => NinaMessageContentType.Update,
            _ => null
        };

        /// <summary> Erzeugt den <see cref="NinaMessageStatusType"/>. </summary>
        public static NinaMessageStatusType? ConvertMessageType(string? messageType) => messageType?.ToUpperInvariant() switch
        {
            "UPDATE" => NinaMessageStatusType.Update,
            _ => null
        };

        /// <summary> Erzeugt den <see cref="NinaSeverityLevel"/>. </summary>
        public static NinaSeverityLevel? ConvertSeverity(string? severity) => severity?.ToUpperInvariant() switch
        {
            "MINOR" => NinaSeverityLevel.Minor,
            "MODERATE" => NinaSeverityLevel.Moderate ,
            "SEVERE" => NinaSeverityLevel.Severe,
            "EXTREME" => NinaSeverityLevel.Extreme,
            _ => null
        };

    }
}