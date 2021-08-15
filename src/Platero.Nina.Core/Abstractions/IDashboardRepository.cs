using System.Threading.Tasks;
using Platero.Nina.Core.Models;

namespace Platero.Nina.Core.Abstractions
{
    /// <summary>
    /// Verwaltet Zugriff auf die Dashboard-Nachrichten eines AGS.
    /// </summary>
    public interface IDashboardRepository
    {
        /// <summary>
        /// Ruft die Nachrichten für einen Kreis ab.
        /// </summary>
        /// <param name="areaCode">Der Gebietscode für eine Gemeindes des Kreises, dessen Nachrichten abgerufen werden. </param>
        /// <returns>Das Dashboard des Kreises der abgerufenen Gemeinde. </returns>
        public Task<NinaDashboard> GetDashboardAsync(AreaCode areaCode);
    }
}