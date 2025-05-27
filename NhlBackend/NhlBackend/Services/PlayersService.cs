using System.Diagnostics;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using NhlBackend.Contracts.Services;
using NhlBackend.Extensions;
using NhlBackend.Models.DataTransferObjects;
using NhlBackend.Models.Enums;
using NhlBackend.Models.Types;
using Npgsql;

namespace NhlBackend.Services;

public class PlayersService : IPlayersService
{
    private readonly string? _connectionString;
    
    public PlayersService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("NhlDatabase");
    }

    public async Task<IReadOnlyCollection<PlayerDto>> GetPlayersOfTeamAsync(
        int teamId, 
        CancellationToken cancellationToken)
    {
        var players = new List<PlayerDto>();
        
        await using var conn = new NpgsqlConnection(_connectionString);
        
        await conn.OpenAsync(cancellationToken);

        const string query = $"""
                                 SELECT p.id, p.first_name, p.last_name, p.birth_date, p.height_sm, p.weight_kg, 
                                        p.shot, p.position
                                 FROM player_contracts pc
                                 JOIN players p ON pc.player_id = p.id
                                 WHERE pc.team_id = @teamId;
                              """;

        await using var cmd = new NpgsqlCommand(query, conn);
        
        cmd.Parameters.AddWithValue("teamId", teamId);

        var stopWatch = Stopwatch.StartNew();
        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        stopWatch.Stop();
        
        MetricsRegistry.DbDuration
            .WithLabels("GetPlayersOfTeamAsync")
            .Observe(stopWatch.Elapsed.TotalSeconds);
        
        while (await reader.ReadAsync(cancellationToken))
        {
            players.Add(new PlayerDto(
                reader.GetInt32(reader.GetOrdinal("id")),
                reader.GetString(reader.GetOrdinal("first_name")),
                reader.GetString(reader.GetOrdinal("last_name")),
                reader.IsDBNull(reader.GetOrdinal("birth_date")) ? null : 
                    DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("birth_date"))),
                reader.IsDBNull(reader.GetOrdinal("height_sm")) ? null : 
                    reader.GetInt16(reader.GetOrdinal("height_sm")),
                reader.IsDBNull(reader.GetOrdinal("weight_kg")) ? null :
                    reader.GetFloat(reader.GetOrdinal("weight_kg")),
                reader.IsDBNull(reader.GetOrdinal("shot")) ? null : 
                    ParseEnumSnakeCase<Shot>(reader.GetString(reader.GetOrdinal("shot"))),
                reader.IsDBNull(reader.GetOrdinal("position")) ? null : 
                    ParseEnumSnakeCase<Position>(reader.GetString(reader.GetOrdinal("position"))))
            );
        }

        return players;
    }

    public async Task<IReadOnlyCollection<AwardDto>> GetPlayerAwardsAsync(
        int playerId, 
        CancellationToken cancellationToken)
    {
        var awards = new List<AwardDto>();
        
        await using var conn = new NpgsqlConnection(_connectionString);
        
        await conn.OpenAsync(cancellationToken);

        const string query = $"""
                                 SELECT a.id, a.name, a.category
                                 FROM player_awards pa
                                 JOIN awards a ON pa.award_id = a.id
                                 WHERE pa.player_id = @playerId;
                              """;

        await using var cmd = new NpgsqlCommand(query, conn);
        
        cmd.Parameters.AddWithValue("playerId", playerId);

        var stopWatch = Stopwatch.StartNew();
        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        stopWatch.Stop();
        
        MetricsRegistry.DbDuration
            .WithLabels("GetPlayerAwardsAsync")
            .Observe(stopWatch.Elapsed.TotalSeconds);
        
        while (await reader.ReadAsync(cancellationToken))
        {
            awards.Add(new AwardDto(
                reader.GetInt32(reader.GetOrdinal("id")),
                reader.GetString(reader.GetOrdinal("name")),
                ParseEnumSnakeCase<AwardCategory>(
                    reader.GetString(reader.GetOrdinal("category"))))
            );
        }

        return awards;
    }

    public async Task<IReadOnlyCollection<PlayerSniperDto>> GetTopSnipersAsync(
        YearOnly startYear,
        int limit,
        CancellationToken cancellationToken)
    {
        var seasonStart = new DateOnly(startYear.Year, 9, 1);
        var seasonEnd = new DateOnly(startYear.Year + 1, 7, 1);

        var snipers = new List<PlayerSniperDto>();
       
        await using var conn = new NpgsqlConnection(_connectionString);
        
        await conn.OpenAsync(cancellationToken);

        var query = $"""
                        WITH season_frame AS (
                        SELECT @startDate::date AS season_start,
                               @endDate::date   AS season_end
                        )
                        SELECT 
                            p.id                               AS player_id,
                            p.first_name || ' ' || p.last_name AS player,
                            t.short_name                       AS team,
                            COUNT(*)                           AS goals_scored
                        FROM season_frame sf
                        JOIN goals            g  ON TRUE
                        JOIN matches          m  ON m.id            = g.match_id
                        JOIN players          p  ON p.id            = g.scoring_player_id
                        LEFT JOIN player_contracts pc
                               ON pc.player_id = p.id
                              AND pc.start_date <= m.match_date
                              AND (pc.end_date IS NULL OR pc.end_date >= m.match_date)
                        LEFT JOIN teams      t  ON t.id            = pc.team_id
                        WHERE m.match_date BETWEEN sf.season_start AND sf.season_end
                          AND m.match_type IN ('regular','playoff')
                        GROUP BY p.id, player, t.short_name
                        ORDER BY goals_scored DESC
                        LIMIT {limit};
                    """;

        await using var cmd = new NpgsqlCommand(query, conn);
        
        cmd.Parameters.AddWithValue("startDate", seasonStart.ToDateTime(TimeOnly.MinValue));
        cmd.Parameters.AddWithValue("endDate",   seasonEnd.ToDateTime(TimeOnly.MaxValue));

        var stopWatch = Stopwatch.StartNew();
        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        stopWatch.Stop();
        
        MetricsRegistry.DbDuration
            .WithLabels("GetTopSnipersAsync")
            .Observe(stopWatch.Elapsed.TotalSeconds);
        
        while (await reader.ReadAsync(cancellationToken))
        {
            snipers.Add(new PlayerSniperDto(
                reader.GetInt32(reader.GetOrdinal("player_id")),
                reader.GetString(reader.GetOrdinal("player")),
                reader.IsDBNull(reader.GetOrdinal("team")) ? null 
                    : reader.GetString(reader.GetOrdinal("team")),
                reader.GetInt32(reader.GetOrdinal("goals_scored"))
            ));
        }

        return snipers;
    }
    
    private static T ParseEnumSnakeCase<T>(string input) where T : struct, Enum
    {
        var formatted = CultureInfo.InvariantCulture.TextInfo
            .ToTitleCase(input.Replace("_", " "))
            .Replace(" ", "");
        
        return Enum.Parse<T>(formatted);
    }
}