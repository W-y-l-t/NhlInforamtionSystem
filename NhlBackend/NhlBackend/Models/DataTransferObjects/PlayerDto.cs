using System.Text.Json.Serialization;
using NhlBackend.Models.Enums;

namespace NhlBackend.Models.DataTransferObjects;

public record PlayerDto(
  [property: JsonPropertyName("id")] int Id,
  [property: JsonPropertyName("first_name")] string Name,
  [property: JsonPropertyName("last_name")] string LastName,
  [property: JsonPropertyName("birth_date")] DateOnly? BirthDate,
  [property: JsonPropertyName("height_sm")] short? HeightSm,
  [property: JsonPropertyName("weight_kg")] float? WeightKg,
  [property: JsonPropertyName("shot")] Shot? Shot,
  [property: JsonPropertyName("position")] Position? Position);
