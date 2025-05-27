using System.Text.Json.Serialization;
using NhlBackend.Models.Enums;

namespace NhlBackend.Models.DataTransferObjects;

public record AwardDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("category")] AwardCategory Category);