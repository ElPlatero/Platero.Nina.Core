using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Platero.Nina.Core.Abstractions;
using Platero.Nina.Core.Abstractions.Models;

namespace Platero.Nina.Core.areaCodes
{
    /// <summary>
    /// Implementiert das <see cref="IAreaCodeRepository"/> für die XÖV-Schnittstelle von www.xrepository.de.
    /// </summary>
    public class XoevAreaCodeRepository : IAreaCodeRepository
    {
        private readonly HttpClient _client;

        /// <summary>
        /// Verwaltet den Zugriff auf die Regionalschlüsseldefinitionen, die von xrepository.de bereitgestellt werden.
        /// </summary>
        /// <param name="client"></param>
        public XoevAreaCodeRepository(HttpClient client)
        {
            _client = client;
        }

        /// <inheritdoc />
        public async Task<ICollection<AreaCode>> GetAreaCodeCollectionAsync()
        {
            var response = await _client.GetAsync(null as Uri);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadFromJsonAsync<GetXoevAreaCodesResponse>();
            if (content?.Data == null) throw new InvalidDataException();
            
            return content.Data.Where(p => p.Length == 3).Select(p => new AreaCode(p[0], p[1], p[2])).ToHashSet();
        }

        #region XÖV-Dto
        //  externally defined data
        #pragma warning disable 1591
        public class GetXoevAreaCodesResponse
        {
            [JsonPropertyName("metadaten")]
            public Metadaten? MetaData { get; set; }
            [JsonPropertyName("spalten")]
            public Spalten[]? ColumnDefinitions { get; set; }
            [JsonPropertyName("daten")]
            public string[][]? Data { get; set; }
        }
        public class Metadaten
        {
            public string? Kennung { get; set; }
            public string? KennungInhalt { get; set; }
            public string? Version { get; set; }
            public string? NameKurz { get; set; }
            public string? NameLang { get; set; }
            public string? NameTechnisch { get; set; }
            public string? HerausgebernameLang { get; set; }
            public string? HerausgebernameKurz { get; set; }
            public string? Beschreibung { get; set; }
            public string? VersionBeschreibung { get; set; }
            public string? AenderungZurVorversion { get; set; }
            public string? HandbuchVersion { get; set; }
            public bool XoevHandbuch { get; set; }
            public long GueltigAb { get; set; }
            public IList<object>? Bezugsorte { get; set; }
        }

        public class Verwendung
        {
            public string? Code { get; set; }
        }

        public class Spalten
        {
            public string? SpaltennameLang { get; set; }
            public string? SpaltennameTechnisch { get; set; }
            public string? Datentyp { get; set; }
            public bool CodeSpalte { get; set; }
            public Verwendung? Verwendung { get; set; }
            public bool EmpfohleneCodeSpalte { get; set; }
        }
        #pragma warning restore 1591
        #endregion
    }
}
    