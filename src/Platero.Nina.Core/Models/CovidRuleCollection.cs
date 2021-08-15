using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Platero.Nina.Core.Models
{
    /// <summary>
    /// Bildet eine Menge von Regeln der Corona-19-Pandemie ab.
    /// </summary>
    public class CovidRuleCollection : ICollection<CovidRule>
    {
        private readonly Dictionary<string, CovidRule> _internalList = new();

        /// <summary>
        /// Erzeugt ein neues Regelset.
        /// </summary>
        /// <param name="areaCode">Der allgemeine Gebietscode, für den die Regeln gelten.</param>
        /// <param name="currentStatus">Der aktuelle Status der Pandemie.</param>
        /// <param name="generalHintHtml">Allgemeine Hinweise zum Verhalten während der Pandemie.</param>
        public CovidRuleCollection(AreaCode areaCode, CovidStatus currentStatus, string generalHintHtml) => (AreaCode, CurrentStatus, GeneralHintHtml) = (areaCode, currentStatus, generalHintHtml);

        /// <summary>
        /// Ruft den allgemeinen Gebietsschlüssel ab, für den die Regeln gelten.
        /// </summary>
        public AreaCode AreaCode { get; }
        
        /// <summary>
        /// Ruft den aktuellen Stand der Corona-Pandemie ab.
        /// </summary>
        public CovidStatus CurrentStatus { get; }
        
        /// <summary>
        /// Ruft die allgemein gültigen Regeln ab.
        /// </summary>
        public string GeneralHintHtml { get; }

        #region ICollection
        /// <inheritdoc />
        public IEnumerator<CovidRule> GetEnumerator() => _internalList.Values.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()=> GetEnumerator();

        /// <inheritdoc />
        public void Add(CovidRule item) => _internalList.Add(item.Id, item);

        /// <inheritdoc />
        public void Clear() => _internalList.Clear();

        /// <inheritdoc />
        public bool Contains(CovidRule item) => _internalList.ContainsKey(item.Id);

        /// <inheritdoc />
        public void CopyTo(CovidRule[] array, int arrayIndex) => _internalList.Values.CopyTo(array, arrayIndex);

        /// <inheritdoc />
        public bool Remove(CovidRule item) => _internalList.Remove(item.Id);

        /// <inheritdoc />
        public int Count => _internalList.Count;

        /// <inheritdoc />
        public bool IsReadOnly => false;
        #endregion
    }

    /// <summary>
    /// Der aktuelle Status der COVID-19-Pandemie.
    /// </summary>
    public class CovidStatus
    {
        /// <summary>
        /// Erzeugt einen neuen Status.
        /// </summary>
        /// <param name="title">Die Zusammenfassung des Status.</param>
        /// <param name="incidencesText">Information über aktuelle Inzidenzen im Bundesland und auf Kreisebene in Textform.</param>
        public CovidStatus(string title, string incidencesText) => (Title, IncidencesText) = (title, incidencesText);
        
        /// <summary>
        /// Die Zusammenfassung.
        /// </summary>
        public string Title { get; }
        
        /// <summary>
        /// Informationen über aktuelle Inzidenzen aus Bundes- und Kreisebene.
        /// </summary>
        public string IncidencesText { get; }
    }

    /// <summary>
    /// Bildet eine legale Grundlage einer Corona-Regelung ab.
    /// </summary>
    public class CovidRegulation
    {
        /// <summary>
        /// Erzeugt eine neue Regel-Grundlage.
        /// </summary>
        /// <param name="source">Welche Legislative verantwortet die Grundlage?</param>
        /// <param name="title">Die Bezeichnung.</param>
        /// <param name="sourceUri">Die Webseite mit weiterführenden Informationen.</param>
        public CovidRegulation(CovidRegulationSource source, string title, Uri sourceUri) => (Source, Title, SourceUri) = (source, title, sourceUri);

        /// <summary>
        /// Ruft den für die Regel verantwortliche Legislative auf.
        /// </summary>
        public CovidRegulationSource Source { get; }
        
        /// <summary>
        /// Der Titel der gesetzlichen Grundlage.
        /// </summary>
        public string Title { get; }
        
        /// <summary>
        /// Verweis zur Internetadresse, an der die Grundlage für die Regelung zu finden ist.
        /// </summary>
        public Uri SourceUri { get; }

        /// <summary>
        /// Beginn der Gültigkeit der Regelung.
        /// </summary>
        public DateTime ValidFrom { get; init; }
        
        /// <summary>
        /// Ende der Gültigkeit der Regelung.
        /// </summary>
        public DateTime ValidUntil { get; init; }
        
        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is CovidRegulation other && Source == other.Source;

        /// <inheritdoc />
        public override int GetHashCode() => (int)Source;
    }

    /// <summary>
    /// Verschiedene Legislativen.
    /// </summary>
    public enum CovidRegulationSource
    {
        /// <summary>
        /// Regelung auf Kreisebene.
        /// </summary>
        [Description("Kreis")]
        District,
        /// <summary>
        /// Regelung auf Bundesland-Ebene.
        /// </summary>
        [Description("Land")]
        FederalState,
        
        /// <summary>
        /// Regelung auf Bundes-Ebene.
        /// </summary>
        [Description("Bund")]
        Country
    }
    
    /// <summary>
    /// Bildet eine gesetzliche Regelung zur COVID-19-Pandemie ab.
    /// </summary>
    public class CovidRule
    {
        /// <summary>
        /// Erzeugt eine neue Regel.
        /// </summary>
        /// <param name="id">Der eindeutige Bezeichner der Regel.</param>
        /// <param name="regulation">Die gesetzliche Grundlage der Regel.</param>
        public CovidRule(string id, CovidRegulation regulation)
        {
            Id = id;
            Regulation = regulation;
        }

        /// <summary>
        /// Die ID der Regel.
        /// </summary>
        public string Id { get; }
        
        /// <summary>
        /// Die Zusammenfassung / der Kontext der Regel.
        /// </summary>
        public string Title { get; init; } = string.Empty;
        
        /// <summary>
        /// Der Inhalt der Regel als HTML.
        /// </summary>
        public string BodyHtml { get; init; } = string.Empty;
        
        /// <summary>
        /// Die gesetzliche Grundlage der Regel.
        /// </summary>
        public CovidRegulation Regulation { get; }
    }
}