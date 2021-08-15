using System;
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
    /// Implementiert das <see cref="IDashboardRepository"/> für die Schnittstelle des Bundesamtes für Bevölkerungsschutz.
    /// </summary>
    public class NinaDashboardRepository : IDashboardRepository
    {
        private readonly HttpClient _client;

        /// <summary>
        /// Erzeugt ein neues Repository für den Zugriff auf aktuelle Warnungen des Bundesamtes für Bevölkerungsschutz auf Kreisebene.
        /// </summary>
        /// <param name="client">Der zu nutzende HttpClient. </param>
        public NinaDashboardRepository(HttpClient client)
        {
            _client = client;
        }
        
        /// <inheritdoc />
        public async Task<NinaDashboard> GetDashboardAsync(AreaCode areaCode)
        {
            if (_client.BaseAddress == null)
            {
                throw new InvalidOperationException("Invalid configuration: there is no base address known for AGS-dashboards.");
            }
            
            var arsDistrict = areaCode.Key[..5].PadRight(12, '0');

            if (!_client.BaseAddress!.TryCombine($"{arsDistrict}.json", out var agsEndpoint))
            {
                throw new InvalidOperationException($"Could not determine endpoint for area \"{areaCode.Key}\".");
            }
            
            var response = await _client.GetAsync(agsEndpoint);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadFromJsonAsync<DashboardMessage[]>();
            return new NinaDashboard(areaCode)
            {
                Messages = content?.Select(Convert).ToHashSet() ?? new()
            };
        }
        
        #region Adaptermethoden

        private static NinaMessage Convert(DashboardMessage dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Id)) throw new InvalidDataException("Message has no id.");
            if (dto.Payload == null) throw new InvalidDataException("Message has no payload.");
            if (!dto.Id.Equals(dto.Payload.Id, StringComparison.InvariantCultureIgnoreCase)) throw new InvalidDataException("ID mismatch between message and payload.");
            if (string.IsNullOrWhiteSpace(dto.Payload.Hash)) throw new InvalidDataException("Message payload has no hash.");
            if (dto.Payload.Data == null) throw new InvalidDataException("Message payload has no data.");

            var type = new NinaMessageType(
                ConvertContentType(dto.Payload.Type) ?? throw new InvalidDataException($"Unknown value for payload type: {dto.Payload.Type}."),
                ConvertMessageType(dto.Payload.Data.MsgType) ?? throw new InvalidDataException($"Unknown value for payload data message type: {dto.Payload.Data.MsgType}"),
                ConvertSeverity(dto.Payload.Data.Severity) ?? throw new InvalidDataException($"Unknown value for payload data severity: {dto.Payload.Data.Severity}")
            );

            return new NinaMessage(type, dto.Id, dto.Payload.Hash)
            {
                Date = dto.Sent,
                Title = dto.Payload?.Data?.Headline ?? throw new InvalidDataException("Message payload data headline has no value."),
                Source = dto.Payload.Data.Provider ?? throw new InvalidDataException("Message payload data provider has no value.")
            };
        }

        private static NinaMessageContentType? ConvertContentType(string? contentType) => contentType?.ToUpperInvariant() switch
        {
            "ALERT" => NinaMessageContentType.Alert,
            _ => null
        };

        private static NinaMessageStatusType? ConvertMessageType(string? messageType) => messageType?.ToUpperInvariant() switch
        {
            "UPDATE" => NinaMessageStatusType.Update,
            _ => null
        };

        private static NinaSeverityLevel? ConvertSeverity(string? severity) => severity?.ToUpperInvariant() switch
        {
            "MINOR" => NinaSeverityLevel.Minor,
            "SEVERE" => NinaSeverityLevel.Severe,
            _ => null
        };

        #endregion
        
        #region Nina-Dashboard-Dto
        //  externally defined data
        // ReSharper disable UnusedAutoPropertyAccessor.Local
        // ReSharper disable UnusedMember.Local
        // ReSharper disable ClassNeverInstantiated.Local
        private class TransKeysDto
        {
            public string? Event { get; set; }
        }

        private class AreaDto
        {
            public string? Type { get; set; }
            public string? Data { get; set; }
        }

        private class DataDto
        {
            public string? Headline { get; set; }
            public string? Provider { get; set; }
            public string? Severity { get; set; }
            public string? MsgType { get; set; }
            public TransKeysDto? TransKeys { get; set; }
            public AreaDto? Area { get; set; }
        }

        private class PayloadDto
        {
            public int Version { get; set; }
            public string? Type { get; set; }
            public string? Id { get; set; }
            public string? Hash { get; set; }
            public DataDto? Data { get; set; }
        }

        private class I18NTitleDto
        {
            public string? De { get; set; }
        }

        private class DashboardMessage
        {
            public string? Id { get; set; }
            public PayloadDto? Payload { get; set; }
            [JsonPropertyName("i18nTitle")]
            public I18NTitleDto? I18NTitle { get; set; }
            public DateTime Sent { get; set; }
        }        
        // ReSharper restore UnusedAutoPropertyAccessor.Local
        // ReSharper restore UnusedMember.Local
        // ReSharper restore ClassNeverInstantiated.Local
        #endregion
    }
}