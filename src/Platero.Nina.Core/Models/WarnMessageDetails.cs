using System.ComponentModel;
using System.Threading.Tasks;
using Platero.Nina.Core.Abstractions;
using Platero.Nina.Core.Abstractions.Enums;

namespace Platero.Nina.Core.Models {
    /// <summary>
    /// Die Details einer Warnmeldung.
    /// </summary>
    public class WarnMessageDetails {
        /// <summary>
        /// Eine Beschreibung der Örtlichkeit(en), die die Warnmeldung betrifft.
        /// </summary>
        public string AreaDescription { get; init; } = string.Empty;

        /// <summary>
        /// Der Titel der Warnmeldung.
        /// </summary>
        public string Title { get; init; } = string.Empty;

        /// <summary>
        /// Die ausführliche Beschreibung der Meldung.
        /// </summary>
        public string Description { get; init; } = string.Empty;

        /// <summary>
        /// Handlungsanweisungen für die Bevölkerung.
        /// </summary>
        public string Instruction { get; init; } = string.Empty;

        /// <summary>
        /// Die Warnstufe.
        /// </summary>
        public NinaSeverityLevel SeverityLevel { get; init; }

        /// <summary>
        /// Die Dringlichkeitsstufe der Warnmeldung.
        /// </summary>
        public WarnMessageUrgency Urgency { get; init; }
    }

    /// <summary>
    /// Die Dringlichkeit einer Warnmeldung.
    /// </summary>
    public enum WarnMessageUrgency {
        /// <summary>
        /// Nicht festgelegt.
        /// </summary>
        Undefined,
        /// <summary>
        /// Dringend
        /// </summary>
        [Description("dringend")]
        Urgent
    }

    /// <summary>
    /// Basisklasse sowohl für die per auf Kreisebene abgerufenen Nachrichten, als auch die über die verschiedenen Datenquellen bundesweit angebotenen Meldungen.
    /// </summary>
    public abstract class WarnMessageBase {
        /// <summary>
        /// Vergibt die über die verschiedenen Anbieter eindeutige ID.
        /// </summary>
        /// <param name="id"></param>
        protected WarnMessageBase(string id) => Id = id;

        /// <summary>
        /// Ruft die vom Herausgeber der Nachricht vergebene ID ab.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Lädt bei Bedarf die Details der Meldung nach.
        /// </summary>
        /// <param name="warnMessageRepository">Das zum Nachladen zu nutzende <see cref="IWarnMessageRepository"/>.</param>
        public async Task LoadDetailsAsync(IWarnMessageRepository warnMessageRepository) {
            Details = await warnMessageRepository.GetWarnMessagesDetailsAsync(this);
        }

        /// <summary>
        /// Ruft die Details der Meldung ab, falls sie geladen wurden.
        /// </summary>
        public WarnMessageDetails? Details { get; private set; }
    }
}