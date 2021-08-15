using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Platero.Nina.Core.Abstractions;
using Platero.Nina.Core.Extensions;
using Platero.Nina.Core.Models;

namespace Platero.Nina.Core.Repositories
{
    /// <summary>
    /// Implementiert das <see cref="ICovidRulesRepository"/> für die Schnittstelle des Bundesamtes für Bevölkerungsschutz.
    /// </summary>
    public class NinaCovidRulesRepository : ICovidRulesRepository
    {
        private readonly HttpClient _client;

        /// <summary>
        /// Erzeugt ein neues Repository für den Zugriff auf aktuelle COVID-19-Regeln des Bundesamtes für Bevölkerungsschutz.
        /// </summary>
        /// <param name="client">Der zu nutzende HttpClient. </param>
        public NinaCovidRulesRepository(HttpClient client)
        {
            _client = client;
        }

        /// <inheritdoc />
        public async Task<CovidRuleCollection> GetCovidRulesAsync(AreaCode areaCode)
        {
            if (_client.BaseAddress == null)
            {
                throw new InvalidOperationException("Invalid configuration: there is no base address known for AGS covid rules.");
            }

            if (!_client.BaseAddress!.TryCombine($"{areaCode.Key}.json", out var agsEndpoint))
            {
                throw new InvalidOperationException($"Could not determine endpoint for area \"{areaCode.Key}\".");
            }

            var response = await _client.GetAsync(agsEndpoint);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadFromJsonAsync<AgsCovidRulesDto>();

            if (content == null) throw new InvalidDataException("Response was empty.");
            if (string.IsNullOrWhiteSpace(content.GeneralInfo)) throw new InvalidDataException("Message has no generalInfo data.");
            if (content.Rules == null) throw new InvalidDataException("Missing rules data.");
            
            var regulations = Convert(content.Regulations);
            var result = new CovidRuleCollection(areaCode, Convert(content.Level), content.GeneralInfo);
            foreach (var ruleDto in content.Rules)
            {
                if (string.IsNullOrWhiteSpace(ruleDto.Id)) throw new InvalidDataException("Missing id in rules.");
                if (string.IsNullOrWhiteSpace(ruleDto.Caption)) throw new InvalidDataException("Missing caption in rules.");
                if (string.IsNullOrWhiteSpace(ruleDto.Text)) throw new InvalidDataException("Missing text in rules.");
                if (string.IsNullOrWhiteSpace(ruleDto.Source)) throw new InvalidDataException("Missing source in rules.");

                var regulation = ruleDto.Source switch
                {
                    "BUND" => regulations[CovidRegulationSource.Country],
                    "LAND" => regulations[CovidRegulationSource.FederalState],
                    "KREIS" => regulations[CovidRegulationSource.District],
                    _ => throw new ArgumentOutOfRangeException(nameof(ruleDto.Source), $"Unknown value {ruleDto.Source} of source.")
                }; 

                result.Add(new CovidRule(ruleDto.Id, regulation)
                {
                    Title = ruleDto.Caption,
                    BodyHtml = ruleDto.Text,
                });
            }
            return result;
        }

        #region Adaptermethoden
        private static CovidStatus Convert(LevelDto? contentLevel)
        {
            if (string.IsNullOrWhiteSpace(contentLevel?.Headline)) throw new InvalidDataException("Message level headline has no data. ");
            if (string.IsNullOrWhiteSpace(contentLevel.Range)) throw new InvalidDataException("Message level range has no data. ");
            return new CovidStatus(contentLevel.Headline, contentLevel.Range);
        }

        private static CovidRegulation Convert(CovidRegulationSource source, RegulationDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Caption ?? dto.Title)) throw new InvalidDataException("Regulation source is missing a caption or title.");
            if (dto.Url == null) throw new InvalidDataException("Missing uri in regulation.");
            return new CovidRegulation(source, dto.Caption ?? dto.Title!, dto.Url);
        }

        private static Dictionary<CovidRegulationSource, CovidRegulation> Convert(RegulationsContainerDto? regulations)
        {
            var result = new Dictionary<CovidRegulationSource, CovidRegulation>();
            if (regulations?.Sections?.Bund != null)
            {
                result.Add(CovidRegulationSource.Country, Convert(CovidRegulationSource.Country, regulations.Sections.Bund));
            }
            if (regulations?.Sections?.Land != null)
            {
                result.Add(CovidRegulationSource.FederalState, Convert(CovidRegulationSource.FederalState, regulations.Sections.Land));
            }
            if (regulations?.Sections?.Kreis != null)
            {
                result.Add(CovidRegulationSource.District, Convert(CovidRegulationSource.District, regulations.Sections.Kreis));
            }

            return result;
        }
        #endregion
        
        #region NINA-AGS-CovidRules-Dto
        //  externally defined data
        // ReSharper disable UnusedAutoPropertyAccessor.Local
        // ReSharper disable UnusedMember.Local
        // ReSharper disable ClassNeverInstantiated.Local
        private class LevelDto
        {
            public string? Headline { get; set; }
            public string? Range { get; set; }
            public string? BackgroundColor { get; set; }
            public string? TextColor { get; set; }
        }

        private class IconDto
        {
            public string? Src { get; set; }
            public string? Hash { get; set; }
        }

        private class RegulationDto
        {
            public string? Title { get; set; }
            public Uri? Url { get; set; }
            public string? Caption { get; set; }
            public DateTime? ValidFrom { get; set; }
            public DateTime? ValidUntil { get; set; }
            public IconDto? Icon { get; set; }
        }

        private class RuleDto
        {
            public string? Id { get; set; }
            public string? Caption { get; set; }
            public string? Text { get; set; }
            public string? Source { get; set; }
            public IconDto? Icon { get; set; }
            public SectionsDto? Links { get; set; }
        }

        private class SectionsDto
        {
            public RegulationDto? Bund { get; set; }
            public RegulationDto? Land { get; set; }
            public RegulationDto? Kreis { get; set; }
        }

        private class RegulationsContainerDto
        {
            public string? ValidFromUntil { get; set; }
            public SectionsDto? Sections { get; set; }
        }

        private class ImprintDto
        {
            public string? Id { get; set; }
            public string? Caption { get; set; }
            public string? Text { get; set; }
        }

        private class AgsCovidRulesDto
        {
            public string? Key { get; set; }
            public LevelDto? Level { get; set; }
            public string? GeneralInfo { get; set; }
            public List<RuleDto>? Rules { get; set; }
            public RegulationsContainerDto? Regulations { get; set; }
            public List<ImprintDto>? Common { get; set; }
        }
        // ReSharper restore UnusedAutoPropertyAccessor.Local
        // ReSharper restore UnusedMember.Local
        // ReSharper restore ClassNeverInstantiated.Local
       #endregion
    }
}