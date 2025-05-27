using System.Text.Json.Serialization;

namespace NhlBackend.Models.DataTransferObjects;

public record TeamDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("full_name")] string FullName,
    [property: JsonPropertyName("short_name")] string ShortName,
    [property: JsonPropertyName("city")] string? City,
    [property: JsonPropertyName("region")] string? Region,
    [property: JsonPropertyName("country")] string? Country,
    [property: JsonPropertyName("founded_date")] DateOnly? FoundedDate,
    [property: JsonPropertyName("entry_date")] DateOnly? EntryDate,
    [property: JsonPropertyName("logo_url")] string? LogoUrl,
    [property: JsonPropertyName("home_uniform_url")] string? HomeUniformUrl,
    [property: JsonPropertyName("away_uniform_url")] string? AwayUniformUrl,
    [property: JsonPropertyName("division_id")] int? DivisionId,
    [property: JsonPropertyName("conference_id")] int? ConferenceId,
    [property: JsonPropertyName("arena_id")] int? ArenaId);
    