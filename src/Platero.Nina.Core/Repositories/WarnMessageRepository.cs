using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;

        /// <summary>
        /// Erzeugt ein neues Repository für den Zugriff auf aktuelle Katastrophen-Warnmeldungen des Bundesamtes für Bevölkerungsschutz.
        /// </summary>
        /// <param name="loggerFactory">Erstellt den Logger.</param>
        /// <param name="client">Der zu nutzende HttpClient. </param>
        public WarnMessageRepository(ILoggerFactory loggerFactory, HttpClient client) {
            _logger = loggerFactory.CreateLogger<WarnMessageRepository>();
            _client = client;
        }

        /// <inheritdoc />
        public async IAsyncEnumerable<WarnMessage> GetWarnMessagesAsync(params WarnMessageType[] sources)
        {
            if (_client.BaseAddress == null)
            {
                throw new InvalidOperationException("Invalid configuration: there is no base address known for AGS-dashboards.");
            }

            if (sources.Any() != true)
            {
                sources = Enum.GetValues<WarnMessageType>();
            }

            HashSet<WarnMessage> result = new();
            foreach (var source in sources)
            {
                foreach (var message in await GetWarnMessagesAsync(source))
                {
                    if (result.Add(message))
                    {
                        yield return message;
                    }
                }
            }
        }

        /// <inheritdoc />
        public async Task<WarnMessageDetails?> GetWarnMessagesDetailsAsync(WarnMessageBase message)
        {
            if (!_client.BaseAddress!.TryCombine($"warnings/{message.Id}.json", out var uri))
            {
                throw new InvalidOperationException($"Could not determine endpoint for warnings from \"{message.Id}\".");
            }

            var details = await _client.GetFromJsonAsync<MapWarningDetailDto>(uri);
            if (details?.Info.Count != 1 || details.Info.Single().Area.Count != 1) {
                _logger.LogWarning("Beim Laden der Details wurde mehr als ein Info-/Area-Objekt gefunden. Jedes weitere nach dem ersten wird ignoriert.");
                return null;
            }

            var info = details.Info.Single();

            return new WarnMessageDetails {
                Title = info.Headline,
                AreaDescription = info.Area.Single().AreaDesc,
                Description = info.Description,
                Instruction = info.Instruction,
                SeverityLevel = Adapter.ConvertSeverity(info.Severity) ?? throw new InvalidDataException($@"Unknown severity value ""{info.Severity}""."),
                Urgency = Adapter.ConvertUrgency(info.Urgency) ?? throw new InvalidDataException($@"Unknown urgency value ""{info.Urgency}""."),
            };
        }

        private async Task<HashSet<WarnMessage>> GetWarnMessagesAsync(WarnMessageType type)
        {
            if (!_client.BaseAddress!.TryCombine($"{type.ToString().ToLowerInvariant()}/mapData.json", out var uri))
            {
                throw new InvalidOperationException($"Could not determine endpoint for warnings from \"{type.ToDescription()}\".");
            }

            var response = await _client.GetFromJsonAsync<MapWarningDto[]>(uri) ?? throw new InvalidDataException("Could not parse response.");
            
            return response.Select(p => new WarnMessage(p.Id ?? throw new InvalidDataException("MapWarning id has no value."))
            {
                Version = p.Version,
                StartDate = p.StartDate,
                Severity = Adapter.ConvertSeverity(p.Severity) ?? throw new InvalidDataException($@"Unknown severity value ""{p.Severity}""."),
                ContentType = Adapter.ConvertContentType(p.Type) ?? throw new InvalidDataException($@"Unknown type value ""{p.Type}""."),
                Content = p.I18NTitle?.De ?? throw new InvalidDataException("Mapwarning has no text.")
            }).ToHashSet();
        }

        #region MapWarning-Dto
        // externally defined data
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

        private class EventCodeDto
        {
            public string? ValueName { get; set; }
            public string? Value { get; set; }
        }

        private class ParameterDto
        {
            public string? ValueName { get; set; }
            public string? Value { get; set; }
        }

        private class GeocodeDto
        {
            public string? ValueName { get; set; }
            public string? Value { get; set; }
        }

        private class AreaDto {
            public string AreaDesc { get; set; } = string.Empty;
            public List<GeocodeDto>? Geocode { get; set; }
        }

        private class InfoDto
        {
            public string? Language { get; set; }
            public List<string>? Category { get; set; }
            [JsonPropertyName("event")]
            public string? EventName { get; set; }
            public string? Urgency { get; set; }
            public string? Severity { get; set; }
            public string? Certainty { get; set; }
            public List<EventCodeDto>? EventCode { get; set; }
            public string Headline { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string Instruction { get; set; } = string.Empty;
            public List<ParameterDto>? Parameter { get; set; }
            public List<AreaDto> Area { get; set; } = new();
        }

        private class MapWarningDetailDto
        {
            public string? Identifier { get; set; }
            public string? Sender { get; set; }
            public DateTime? Sent { get; set; }
            public string? Status { get; set; }
            public string? MsgType { get; set; }
            public string? Scope { get; set; }
            public List<string>? Code { get; set; }
            public string? References { get; set; }
            public List<InfoDto> Info { get; set; } = new();
        }

        // ReSharper restore UnusedAutoPropertyAccessor.Local
        // ReSharper restore UnusedMember.Local
        // ReSharper restore ClassNeverInstantiated.Local
        #endregion
    }
}