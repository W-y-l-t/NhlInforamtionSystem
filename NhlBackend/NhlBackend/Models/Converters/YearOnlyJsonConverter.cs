using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using NhlBackend.Models.Types;

namespace NhlBackend.Models.Converters;

public class YearOnlyJsonConverter : JsonConverter<YearOnly>
{
    private const string YearFormat = "yyyy";

    public override YearOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();

        if (str != null && YearOnly.TryParseExact(
                str, YearFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var yearOnly))
        {
            return yearOnly;
        }
        
        throw new JsonException($"Year format must be {YearFormat}.");
    }

    public override void Write(Utf8JsonWriter writer, YearOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}