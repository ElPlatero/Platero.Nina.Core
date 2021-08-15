using System.ComponentModel;

namespace Platero.Nina.Core.Abstractions.Enums
{
    /// <summary>
    /// Die verschiedenen Systeme, die Warnmeldungen produzieren.
    /// </summary>
    public enum WarnMessageType
    {
        /// <summary>
        /// Meldungen aus KatWarn.
        /// </summary>
        [Description("KatWarn-Meldungen")]
        KatWarn,
        
        /// <summary>
        /// Meldungen aus BiwApp
        /// </summary>
        [Description("BiwApp-Meldungen")]
        BiwApp,
        
        /// <summary>
        /// Meldungen aus MoWas.
        /// </summary>
        [Description("MoWas-Meldungen")]
        MoWas,
        
        /// <summary>
        /// Meldungen des Deutschen Wetterdienstes.
        /// </summary>
        [Description("Deutscher Wetterdienst")]
        Dwd,
        /// <summary>
        /// Meldungen vom Hochwasserportal.
        /// </summary>
        [Description("Länderübergreifendes Hochwasserportal")]
        Lhp
    }
}