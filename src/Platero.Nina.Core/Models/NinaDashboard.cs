using System;
using System.Collections.Generic;

namespace Platero.Nina.Core.Models
{
    /// <summary>
    /// Eine Meldungsübersicht für einen amtlichen Gebietsschlüssel auf Kreisebene.
    /// </summary>
    public class NinaDashboard
    {
        /// <summary>
        /// Erzeugt eine neue Meldungsübersicht.
        /// </summary>
        /// <param name="areaCode">Der amtliche Gebietsschlüssel, für den die Meldungen gelten.</param>
        public NinaDashboard(AreaCode areaCode)
        {
            AreaCode = areaCode;
        }

        /// <summary>
        /// Der amtliche Gebietsschlüssel, für den diese Warnungen gelten.
        /// </summary>
        public AreaCode AreaCode { get; }

        /// <summary>
        /// Eine Liste der aktuellen Meldungen. 
        /// </summary>
        public ICollection<NinaMessage> Messages { get; set; } = ArraySegment<NinaMessage>.Empty;
    }
}