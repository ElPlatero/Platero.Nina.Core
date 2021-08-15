using System.Collections.Generic;
using System.Threading.Tasks;
using Platero.Nina.Core.Models;

namespace Platero.Nina.Core.Abstractions
{
    /// <summary>
    /// Verwaltet den Zugriff auf die aktuellen Katastrophen-Warnmeldungen des Bundesamts für Bevölkerungsschutz.
    /// </summary>
    public interface IKatWarnMessageRepository
    {
        /// <summary>
        /// Ruft die aktuellen Katastrophen-Warnmeldungen des Bundesamts für Bevölkerungsschutz ab. 
        /// </summary>
        /// <returns>Eine Menge von Katastrophen-Warnmeldungen.</returns>
        public Task<ICollection<KatWarnMessage>> GetKatWarnMessages();
    }
}