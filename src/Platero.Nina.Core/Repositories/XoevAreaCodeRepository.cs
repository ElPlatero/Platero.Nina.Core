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
using Platero.Nina.Core.Json;

namespace Platero.Nina.Core.Repositories
{
    /// <summary>
    /// Implementiert das <see cref="IAreaCodeRepository"/> für die XÖV-Schnittstelle von www.xrepository.de.
    /// </summary>
    public class XoevAreaCodeRepository : IAreaCodeRepository
    {
        private readonly HttpClient _client;

        /// <summary>
        /// Erzeugt ein neues Repository für den Zugriff auf Regionalcodes.
        /// </summary>
        /// <param name="client">Der zu nutzende HttpClient.</param>
        public XoevAreaCodeRepository(HttpClient client)
        {
            _client = client;
        }

        /// <inheritdoc />
        public async Task<HashSet<AreaCode>> GetAreaCodeSetAsync()
        {
            var response = await _client.GetAsync(null as Uri);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadFromJsonAsync<GetXoevAreaCodesResponse>();
            if (content?.Data == null) throw new InvalidDataException();
            
            return content.Data.Where(p => p.Length == 3).Select(p => new AreaCode(p[0], p[1], p[2])).ToHashSet();
        }

        #region XÖV-Dto
        //  externally defined data
        // ReSharper disable UnusedAutoPropertyAccessor.Local
        // ReSharper disable UnusedMember.Local
        // ReSharper disable ClassNeverInstantiated.Local
        private class GetXoevAreaCodesResponse
        {
            [JsonPropertyName("metadaten")]
            public Metadaten? MetaData { get; set; }
            [JsonPropertyName("spalten")]
            public Spalten[]? ColumnDefinitions { get; set; }
            [JsonPropertyName("daten")]
            public string[][]? Data { get; set; }
        }
        private class Metadaten
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
            [JsonConverter(typeof(UnixTimestampConverter))]
            public DateTime GueltigAb { get; set; }
            public IList<object>? Bezugsorte { get; set; }
        }

        private class Verwendung
        {
            public string? Code { get; set; }
        }

        private class Spalten
        {
            public string? SpaltennameLang { get; set; }
            public string? SpaltennameTechnisch { get; set; }
            public string? Datentyp { get; set; }
            public bool CodeSpalte { get; set; }
            public Verwendung? Verwendung { get; set; }
            public bool EmpfohleneCodeSpalte { get; set; }
        }
        // ReSharper restore UnusedAutoPropertyAccessor.Local
        // ReSharper restore UnusedMember.Local
        // ReSharper restore ClassNeverInstantiated.Local
        #endregion
    }
}
    