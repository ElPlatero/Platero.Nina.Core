﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Platero.Nina.Core.Abstractions.Enums;
using Platero.Nina.Core.Models;

namespace Platero.Nina.Core.Abstractions
{
    /// <summary>
    /// Verwaltet den Zugriff auf die aktuellen Katastrophen-Warnmeldungen des Bundesamts für Bevölkerungsschutz.
    /// </summary>
    public interface IWarnMessageRepository
    {
        /// <summary>
        /// Ruft die aktuellen Katastrophen-Warnmeldungen des Bundesamts für Bevölkerungsschutz ab. 
        /// </summary>
        /// <param name="sources">Die Quellen, deren Meldungen berücksichigt werden sollen.</param>
        /// <returns>Eine Menge von Katastrophen-Warnmeldungen.</returns>
        public Task<ICollection<WarnMessage>> GetWarnMessages(params WarnMessageType[] sources);
    }
}