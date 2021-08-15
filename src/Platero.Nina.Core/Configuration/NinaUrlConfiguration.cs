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

        /// <summary>
        /// Die URL für den Abruf von COVID-19-Regeln.
        /// </summary>
        public Uri? NinaCovidRules { get; set; }
    }
}