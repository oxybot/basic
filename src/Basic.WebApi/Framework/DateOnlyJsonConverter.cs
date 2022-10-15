using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Basic.WebApi.Framework
{
    /// <summary>
    /// Converts <see cref="DateOnly"/> value to string as part of a json serialization.
    /// </summary>
    /// <remarks>
    /// Values are serialized a string, using a <c>yyyy-MM-dd</c> format.
    /// </remarks>
    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        /// <summary>
        /// The format used to write or read a <see cref="DateOnly"/> value from a json payload.
        /// </summary>
        public const string Format = "yyyy-MM-dd";

        /// <summary>
        /// Reads a json value as a <see cref="DateOnly"/> value.
        /// </summary>
        /// <param name="reader">The current json reader.</param>
        /// <param name="typeToConvert">The parameter is not used.</param>
        /// <param name="options">The parameter is not used.</param>
        /// <returns>The <see cref="DateOnly"/> value extracted from the reader.</returns>
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeToConvert is null)
            {
                throw new ArgumentNullException(nameof(typeToConvert));
            }

            return DateOnly.ParseExact(reader.GetString(), Format, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Writes a <see cref="DateOnly"/> value into a json payload.
        /// </summary>
        /// <param name="writer">The current json writer.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="options">The parameter is not used.</param>
        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStringValue(value.ToString(Format, CultureInfo.InvariantCulture));
        }
    }
}
