using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using NhlBackend.Contracts.Services;
using NhlBackend.Extensions;
using NhlBackend.Models.DataTransferObjects;
using NhlBackend.Models.Types;
using Npgsql;

namespace NhlBackend.Services;

public class TeamsService : ITeamsService
{
    private readonly string? _connectionString;

    public TeamsService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("NhlDatabase");
    }

    public async Task<IReadOnlyCollection<TeamDto>> GetAllTeamsAsync(CancellationToken cancellationToken)
    {
        var teams = new List<TeamDto>();

        await using var conn = new NpgsqlConnection(_connectionString);

        await conn.OpenAsync(cancellationToken);

        const string query = $"""
                                 SELECT id, full_name, short_name, city, region, country,
                                        founded_date, entry_date, logo_url,
                                        home_uniform_url, away_uniform_url,
                                        division_id, conference_id, arena_id
                                 FROM teams;
                              """;

        await using var cmd = new NpgsqlCommand(query, conn);

        var stopWatch = Stopwatch.StartNew();
        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        stopWatch.Stop();

        MetricsRegistry.DbDuration
            .WithLabels("GetAllTeamsAsync")
            .Observe(stopWatch.Elapsed.TotalSeconds);

        while (await reader.ReadAsync(cancellationToken))
        {
            teams.Add(new TeamDto(
                reader.GetInt32(reader.GetOrdinal("id")),
                reader.GetString(reader.GetOrdinal("full_name")),
                reader.GetString(reader.GetOrdinal("short_name")),
                reader.IsDBNull(reader.GetOrdinal("city")) ? null : reader.GetString(reader.GetOrdinal("city")),
                reader.IsDBNull(reader.GetOrdinal("region")) ? null : reader.GetString(reader.GetOrdinal("region")),
                reader.IsDBNull(reader.GetOrdinal("country")) ? null : reader.GetString(reader.GetOrdinal("country")),
                reader.IsDBNull(reader.GetOrdinal("founded_date"))
                    ? null
                    : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("founded_date"))),
                reader.IsDBNull(reader.GetOrdinal("entry_date"))
                    ? null
                    : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("entry_date"))),
                reader.IsDBNull(reader.GetOrdinal("logo_url")) ? null : reader.GetString(reader.GetOrdinal("logo_url")),
                reader.IsDBNull(reader.GetOrdinal("home_uniform_url"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("home_uniform_url")),
                reader.IsDBNull(reader.GetOrdinal("away_uniform_url"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("away_uniform_url")),
                reader.IsDBNull(reader.GetOrdinal("division_id"))
                    ? null
                    : reader.GetInt32(reader.GetOrdinal("division_id")),
                reader.IsDBNull(reader.GetOrdinal("conference_id"))
                    ? null
                    : reader.GetInt32(reader.GetOrdinal("conference_id")),
                reader.IsDBNull(reader.GetOrdinal("arena_id")) ? null : reader.GetInt32(reader.GetOrdinal("arena_id"))
            ));
        }

        return teams;
    }

    public async Task<TeamPerformanceDto> GetTeamPerformanceAsync(
        int teamId, YearOnly season, CancellationToken token)
    {
        var year = season.Year;
        var start = new DateOnly(year,   9,  1);
        var end = new DateOnly(year + 1, 7,  1);

        const string perfSql = """
                                   WITH season_matches AS (
                                     SELECT m.id,
                                            m.home_team_id,
                                            m.away_team_id,
                                            m.home_team_score,
                                            m.away_team_score
                                     FROM matches m
                                     WHERE m.match_date BETWEEN @start AND @end
                                       AND (m.home_team_id = @teamId OR m.away_team_id = @teamId)
                                   )
                                   SELECT
                                     @teamId AS team_id,
                                     @season AS season,
                                     COUNT(*)                                                  AS games_played,
                                     COALESCE(
                                       SUM(
                                         CASE WHEN (sm.home_team_id = @teamId AND sm.home_team_score > sm.away_team_score)
                                                OR (sm.away_team_id = @teamId AND sm.away_team_score > sm.home_team_score)
                                              THEN 1 ELSE 0 END
                                       ), 0
                                     )                                                          AS wins,
                                     COALESCE(
                                       SUM(
                                         CASE WHEN (sm.home_team_id = @teamId AND sm.home_team_score < sm.away_team_score)
                                                OR (sm.away_team_id = @teamId AND sm.away_team_score < sm.home_team_score)
                                              THEN 1 ELSE 0 END
                                       ), 0
                                     )                                                          AS losses,
                                     COALESCE(SUM(gf.goals_for),    0)                          AS goals_for,
                                     COALESCE(SUM(ga.goals_against),0)                          AS goals_against
                                   FROM season_matches sm
                                   LEFT JOIN (
                                     SELECT g.match_id, COUNT(*) AS goals_for
                                     FROM goals g
                                     WHERE g.team_id = @teamId
                                       AND g.match_id IN (SELECT id FROM season_matches)
                                     GROUP BY g.match_id
                                   ) gf ON gf.match_id = sm.id
                                   LEFT JOIN (
                                     SELECT g.match_id, COUNT(*) AS goals_against
                                     FROM goals g
                                     WHERE g.team_id <> @teamId
                                       AND g.match_id IN (SELECT id FROM season_matches)
                                     GROUP BY g.match_id
                                   ) ga ON ga.match_id = sm.id;
                               """;

        await using var conn = new NpgsqlConnection(_connectionString);
        
        await conn.OpenAsync(token);
        
        await using var cmd = new NpgsqlCommand(perfSql, conn);
        cmd.Parameters.AddWithValue("teamId", teamId);
        cmd.Parameters.AddWithValue("season", season.ToString());
        cmd.Parameters.AddWithValue("start", start.ToDateTime(TimeOnly.MinValue));
        cmd.Parameters.AddWithValue("end",   end  .ToDateTime(TimeOnly.MaxValue));

        var stopWatch = Stopwatch.StartNew();
        await using var reader = await cmd.ExecuteReaderAsync(token);
        stopWatch.Stop();
        
        MetricsRegistry.DbDuration.WithLabels("GetTeamPerformance")
                                  .Observe(stopWatch.Elapsed.TotalSeconds);

        if (!await reader.ReadAsync(token))
        {
            throw new InvalidOperationException("No data for the given season.");
        }

        return new TeamPerformanceDto(
            reader.GetInt32(reader.GetOrdinal("team_id")),
            reader.GetString(reader.GetOrdinal("season")),
            reader.GetInt32(reader.GetOrdinal("games_played")),
            reader.GetInt32(reader.GetOrdinal("wins")),
            reader.GetInt32(reader.GetOrdinal("losses")),
            reader.GetInt32(reader.GetOrdinal("goals_for")),
            reader.GetInt32(reader.GetOrdinal("goals_against"))
        );
    }
}