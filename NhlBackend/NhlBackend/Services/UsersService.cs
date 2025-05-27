using System.Diagnostics;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using NhlBackend.Contracts.Services;
using NhlBackend.Extensions;
using NhlBackend.Models.DataTransferObjects;
using NhlBackend.Models.Enums;
using Npgsql;

namespace NhlBackend.Services;

public class UsersService : IUsersService
{
    private readonly string? _connectionString;
    
    public UsersService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("NhlDatabase");
    }

    public async Task<IReadOnlyCollection<TicketDto>> GetTicketsOfUserAsync(
        long userId, CancellationToken cancellationToken)
    {
        var tickets = new List<TicketDto>();
        
        await using var conn = new NpgsqlConnection(_connectionString);
        
        await conn.OpenAsync(cancellationToken);

        const string query = $"""
                                 SELECT tk.id, tk.match_id, tk.seat_id, tk.category, tk.price_usd, tk.status
                                 FROM orders o
                                 JOIN order_items oi ON oi.order_id = o.id
                                 JOIN tickets tk ON oi.ticket_id = tk.id
                                 WHERE o.user_id = @userId;
                              """;

        await using var cmd = new NpgsqlCommand(query, conn);
        
        cmd.Parameters.AddWithValue("userId", userId);

        var stopWatch = Stopwatch.StartNew();
        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        stopWatch.Stop();
        
        MetricsRegistry.DbDuration
            .WithLabels("GetTicketsOfUserAsync")
            .Observe(stopWatch.Elapsed.TotalSeconds);
        
        while (await reader.ReadAsync(cancellationToken))
        {
            tickets.Add(new TicketDto(
                reader.GetInt32(reader.GetOrdinal("id")),
                reader.GetInt32(reader.GetOrdinal("match_id")),
                reader.GetInt32(reader.GetOrdinal("seat_id")),
                reader.IsDBNull(reader.GetOrdinal("category")) ? null : 
                    reader.GetString(reader.GetOrdinal("category")),
                reader.IsDBNull(reader.GetOrdinal("price_usd")) ? null : 
                    (float)reader.GetDecimal(reader.GetOrdinal("price_usd")),
                reader.IsDBNull(reader.GetOrdinal("status")) ? null : 
                    ParseEnumSnakeCase<TicketStatus>(reader.GetString(reader.GetOrdinal("status"))))
            );
        }

        return tickets;
    }

    public async Task<IReadOnlyCollection<FavoriteTeamDto>> GetFavoriteTeamsAsync(
        long userId, CancellationToken cancellationToken)
    {
        const string favSql = """
                                  SELECT 
                                    t.id,
                                    t.full_name,
                                    t.short_name,
                                    a.name AS arena,
                                    a.capacity AS arena_capacity,
                                    d.division_name,
                                    c.conference_name
                                  FROM user_favorite_teams uft
                                  JOIN teams t ON uft.team_id = t.id
                                  LEFT JOIN arenas a ON t.arena_id = a.id
                                  LEFT JOIN divisions d ON t.division_id = d.id
                                  LEFT JOIN conferences c ON t.conference_id = c.id
                                  WHERE uft.user_id = @userId;
                              """;
        
        var list = new List<FavoriteTeamDto>();
        
        await using var conn = new NpgsqlConnection(_connectionString);
        
        await conn.OpenAsync(cancellationToken);
        
        await using var cmd = new NpgsqlCommand(favSql, conn);
        cmd.Parameters.AddWithValue("userId", userId);

        var stopWatch = Stopwatch.StartNew();
        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        stopWatch.Stop();
        
        MetricsRegistry.DbDuration
            .WithLabels("GetFavoriteTeams")
            .Observe(stopWatch.Elapsed.TotalSeconds);

        while (await reader.ReadAsync(cancellationToken))
        {
            list.Add(new FavoriteTeamDto(
                reader.GetInt32(reader.GetOrdinal("id")),
                reader.GetString(reader.GetOrdinal("full_name")),
                reader.GetString(reader.GetOrdinal("short_name")),
                reader.IsDBNull(reader.GetOrdinal("arena")) ? null : 
                    reader.GetString(reader.GetOrdinal("arena")),
                reader.IsDBNull(reader.GetOrdinal("arena_capacity")) ? null : 
                    reader.GetOrdinal("arena_capacity"),
                reader.IsDBNull(reader.GetOrdinal("division_name")) ? null : 
                    reader.GetString(reader.GetOrdinal("division_name")),
                reader.IsDBNull(reader.GetOrdinal("conference_name")) ? null : 
                    reader.GetString(reader.GetOrdinal("conference_name"))
            ));
        }

        return list;
    }

    private static T ParseEnumSnakeCase<T>(string input) where T : struct, Enum
    {
        var formatted = CultureInfo.InvariantCulture.TextInfo
            .ToTitleCase(input.Replace("_", " "))
            .Replace(" ", "");
        
        return Enum.Parse<T>(formatted);
    }
}