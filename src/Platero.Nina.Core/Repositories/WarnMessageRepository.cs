using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Platero.Nina.Core.Abstractions;
using Platero.Nina.Core.Abstractions.Enums;
using Platero.Nina.Core.Extensions;
using Platero.Nina.Core.Models;

namespace Platero.Nina.Core.Repositories
{
    /// <summary>
    /// Implementiert das <see cref="IWarnMessageRepository"/> für die Schnittstelle des Bundesamtes für Bevölkerungsschutz.
    /// </summary>
    public class WarnMessageRepository : IWarnMessageRepository
    {
        private readonly HttpClient _client;

        /// <summary>
        /// Erzeugt ein neues Repository für den Zugriff auf aktuelle Katastrophen-Warnmeldungen des Bundesamtes für Bevölkerungsschutz.
        /// </summary>
        /// <param name="client">Der zu nutzende HttpClient. </param>
        public WarnMessageRepository(HttpClient client)
        {
            _client = client;
        }

        /// <inheritdoc />
        public async Task<ICollection<WarnMessage>> GetWarnMessages(WarnMessageType type = WarnMessageType.MoWas)
        {
            if (_client.BaseAddress == null)
            {
                throw new InvalidOperationException("Invalid configuration: there is no base address known for AGS-dashboards.");
            }

            if (!_client.BaseAddress.TryCombine($"{type.ToString().ToLowerInvariant()}/mapData.json", out var uri))
            {
                throw new InvalidOperationException($"Could not determine endpoint for warnings from \"{type.ToDescription()}\".");
            }

            var response = await _client.GetFromJsonAsync<MapWarningDto[]>(uri) ?? throw new InvalidDataException("Could not parse response.");
            
            return response.Select(p => new WarnMessage(p.Id ?? throw new InvalidDataException("MapWarning is has no value."))
            {
                Version = p.Version,
                StartDate = p.StartDate,
                Severity = Adapter.ConvertSeverity(p.Severity) ?? throw new InvalidDataException("Mapwarning severity has no value."),
                ContentType = Adapter.ConvertContentType(p.Type) ?? throw new InvalidDataException("Mapwarning type has no value."),
                Content = p.I18NTitle?.De ?? throw new InvalidDataException("Mapwarning has no text.")
            }).ToHashSet();
        }

        
        #region MapWarning-Dto
        //  externally defined data
        // ReSharper disable UnusedAutoPropertyAccessor.Local
        // ReSharper disable UnusedMember.Local
        // ReSharper disable ClassNeverInstantiated.Local
        private class MapWarningDto
        {
            public string? Id { get; set; }
            public int Version { get; set; }
            public DateTime StartDate { get; set; }
            public string? Severity { get; set; }
            public string? Type { get; set; }
            [JsonPropertyName("i18nTitle")]
            public I18NTitle? I18NTitle { get; set; }
        }
        
        private class I18NTitle
        {
            public string? De { get; set; }
        }
        // ReSharper restore UnusedAutoPropertyAccessor.Local
        // ReSharper restore UnusedMember.Local
        // ReSharper restore ClassNeverInstantiated.Local
        #endregion
    }
}