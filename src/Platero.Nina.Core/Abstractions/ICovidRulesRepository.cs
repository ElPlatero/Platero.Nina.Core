using System.Threading.Tasks;
using Platero.Nina.Core.Abstractions.Models;

namespace Platero.Nina.Core.Abstractions
{
    /// <summary>
    /// Verwaltet Zugriff auf die Dashboard-Nachrichten eines AGS.
    /// </summary>
    public interface ICovidRulesRepository
    {
        /// <summary>
        /// Ruft die COVID-19-Regeln für ein amtliches Gebiet ab.
        /// </summary>
        /// <param name="areaCode">Der Gebietscode der Region, für den die Regeln abgerufen werden. </param>
        /// <returns> Die aktuell gültigen COVID-19-Regeln. </returns>
        public Task<CovidRuleCollection> GetCovidRulesAsync(AreaCode areaCode);
    }
}