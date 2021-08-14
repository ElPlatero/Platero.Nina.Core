using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Platero.Nina.Core.Abstractions.Models;

namespace Platero.Nina.Core.Abstractions
{
    /// <summary>
    /// Bietet Zugriff auf Regionalschlüssel (Gemeinden, dargestellt durch den amtlichen Regionalschlüssel ARS
    /// des statistischen Bundesamtes.
    /// </summary>
    public interface IAreaCodeRepository
    {
        /// <summary>
        /// Ruft die Regionalschlüssel ab.
        /// </summary>
        /// <returns> Eine Menge von Regionalschlüsseln. </returns>
        public Task<ICollection<AreaCode>> GetAreaCodeCollectionAsync();
    }
}