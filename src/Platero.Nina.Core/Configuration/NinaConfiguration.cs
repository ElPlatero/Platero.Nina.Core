namespace Platero.Nina.Core.Configuration
{
    /// <summary>
    /// Die Konfiguration der API.
    /// </summary>
    public class NinaConfiguration
    {
        /// <summary>
        /// Ruft die konfigurierten APIs ab, oder legt sie fest.
        /// </summary>
        public NinaUrlConfiguration? Urls { get; set; }
    }
}