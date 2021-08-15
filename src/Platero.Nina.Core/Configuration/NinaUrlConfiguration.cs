using System;

namespace Platero.Nina.Core.Configuration
{
    /// <summary>
    /// Eine Sammlung konfigurierter APIs.
    /// </summary>
    public class NinaUrlConfiguration
    {
        /// <summary>
        /// Die URL für die Regionalschlüssel-Stammdaten.
        /// </summary>
        public Uri? AreaCodes { get; set; }

        /// <summary>
        /// Die URL für die Warnungen auf Kreisebene.
        /// </summary>
        public Uri? NinaDashboard { get; set; }
    }
}