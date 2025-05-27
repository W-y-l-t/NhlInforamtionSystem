using System.Text.Json.Serialization;
using NhlBackend.Models.Enums;

namespace NhlBackend.Models.DataTransferObjects;

public record TicketDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("match_id")] int MatchId,
    [property: JsonPropertyName("seat_id")] int SeatId,
    [property: JsonPropertyName("category")] string? Category,
    [property: JsonPropertyName("price")] float? Price,
    [property: JsonPropertyName("status")] TicketStatus? Status);