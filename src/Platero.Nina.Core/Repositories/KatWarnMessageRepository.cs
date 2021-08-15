﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Platero.Nina.Core.Abstractions;
using Platero.Nina.Core.Abstractions.Enums;
using Platero.Nina.Core.Models;

namespace Platero.Nina.Core.Repositories
{
    /// <summary>
    /// Implementiert das <see cref="IKatWarnMessageRepository"/> für die Schnittstelle des Bundesamtes für Bevölkerungsschutz.
    /// </summary>
    public class KatWarnMessageRepository : IKatWarnMessageRepository
    {
        private readonly HttpClient _client;

        /// <summary>
        /// Erzeugt ein neues Repository für den Zugriff auf aktuelle Katastrophen-Warnmeldungen des Bundesamtes für Bevölkerungsschutz.
        /// </summary>
        /// <param name="client">Der zu nutzende HttpClient. </param>
        public KatWarnMessageRepository(HttpClient client)
        {
            _client = client;
        }
        
        /// <inheritdoc />
        public async Task<ICollection<KatWarnMessage>> GetKatWarnMessages()
        {
            if (_client.BaseAddress == null)
            {
                throw new InvalidOperationException("Invalid configuration: there is no base address known for AGS-dashboards.");
            }

            var response = await _client.GetFromJsonAsync<MapWarningDto[]>(null as Uri) ?? throw new InvalidDataException("Could not parse response.");
            
            return response.Select(p => new KatWarnMessage(p.Id ?? throw new InvalidDataException("MapWarning is has no value."))
            {
                Version = p.Version,
                StartDate = p.StartDate,
                Severity = Adapter.ConvertSeverity(p.Severity) ?? throw new InvalidDataException("Mapwarning severity has no value."),
                ContentType = Adapter.ConvertContentType(p.Type) ?? throw new InvalidDataException("Mapwarning type has no value."),
                Content = p.I18NTitle?.De ?? throw new InvalidDataException("Mapwarning has no text.")
            }).ToHashSet();
        }

        
        #region Nina-KatWarn-MapWarning-Dto
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