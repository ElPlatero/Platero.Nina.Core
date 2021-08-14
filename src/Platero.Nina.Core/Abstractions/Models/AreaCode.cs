using System;

namespace Platero.Nina.Core.Abstractions.Models
{
    /// <summary>
    /// Entspricht dem amtlichen Regionalschlüssel (ARS) des statistischen Bundesamtes.
    /// </summary>
    public class AreaCode
    {
        /// <summary>
        /// Erstellt einen neuen Regionalcode für ein Gebiet.
        /// </summary>
        /// <param name="key"> Der amtliche Regionalschlüssel des statistischen Bundesamtes. </param>
        /// <param name="name"> Die Bezeichnung der Region. </param>
        /// <param name="comment"> Der Hinweis für die Region. </param>
        public AreaCode(string key, string name, string? comment = null) => (Key, Name, Comment) = (key, name, comment);

        /// <summary>
        /// Ruft den amtlichen Regionalschlüssel ab.
        /// </summary>
        public string Key { get; }
        
        /// <summary>
        /// Ruft die Bezeichnung des Gebiets ab.
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// Ruft den Hinweis zum Gebiet ab.
        /// </summary>
        public string? Comment { get; set; }

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is AreaCode areaCode && areaCode.Key.Equals(Key, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        public override int GetHashCode() => Key.GetHashCode();
    }
}