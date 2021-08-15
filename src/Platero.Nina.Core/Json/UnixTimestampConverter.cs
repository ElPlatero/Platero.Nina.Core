using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Platero.Nina.Core.Json
{
    /// <summary>
    /// Konvertiert einen Timestamp (Zeit seit 01.01.1970 UTC in Millisekunden) zu einer lokalen Datumsangabe.
    /// </summary>
    public class UnixTimestampConverter : JsonConverter<DateTime>
    {
        /// <inheritdoc />
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var rawValue = reader.GetInt64();
            return rawValue.ToString().Length > 10
                ? DateTimeOffset.FromUnixTimeMilliseconds(rawValue).LocalDateTime
                : DateTimeOffset.FromUnixTimeSeconds(rawValue).LocalDateTime;
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options) =>
            writer.WriteNumberValue(new DateTimeOffset(value).ToUniversalTime().ToUnixTimeMilliseconds());
    }
}